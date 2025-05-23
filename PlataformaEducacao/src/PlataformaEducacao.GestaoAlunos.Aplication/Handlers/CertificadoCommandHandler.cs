using MediatR;
using PlataformaEducacao.Core.Messages;
using PlataformaEducacao.Core.Messages.IntegrationQueries;
using PlataformaEducacao.Core.Messages.Notifications;
using PlataformaEducacao.GestaoAlunos.Aplication.Commands;
using PlataformaEducacao.GestaoAlunos.Domain;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Handlers;

public class CertificadoCommandHandler(IMediator mediator, 
                                       IAlunoRepository alunoRepository,
                                       ICertificadoPdfService certificadoPdfService) : IRequestHandler<AdicionarCertificadoCommand, bool>
{
    public async Task<bool> Handle(AdicionarCertificadoCommand request, CancellationToken cancellationToken)
    {
        if (!ValidarComando(request))
            return false;

        var aluno = await alunoRepository.ObterPorId(request.AlunoId);
        if (aluno == null)
        {
            await mediator.Publish(new DomainNotification(request.MessageType, "Aluno não encontrado."), cancellationToken);
            return false;
        }
        var matricula = await alunoRepository.ObterMatriculaPorCursoEAlunoId(request.CursoId, request.AlunoId);
        if (matricula == null)
        {
            await mediator.Publish(new DomainNotification(request.MessageType, "Matrícula não encontrada."), cancellationToken);
            return false;
        }

        var curso = await mediator.Send(new ObterCursoQuery(request.CursoId), cancellationToken);
        if (curso == null)
        {
            await mediator.Publish(new DomainNotification(request.MessageType, "Curso não encontrado."), cancellationToken);
            return false;
        }

        var certificado = new Certificado(aluno.Nome, curso.Nome, matricula.Id, aluno.Id, matricula.DataConclusao);

        var pdf = certificadoPdfService.GerarPdf(certificado);

        if (!pdf.Any())
        {
            await mediator.Publish(new DomainNotification(request.MessageType, "Erro ao gerar o PDF do certificado."), cancellationToken);
            return false;
        }

        certificado.AdicionarArquivo(pdf);

        aluno.AdicionarCertificado(certificado);
        alunoRepository.AdicionarCertificado(certificado);
        
        return await alunoRepository.UnitOfWork.Commit();
    }

    private bool ValidarComando(Command command)
    {
        if(command.EhValido())
            return true;

        foreach (var erro in command.ValidationResult.Errors)
        {
           mediator.Publish(new DomainNotification(command.MessageType, erro.ErrorMessage), CancellationToken.None);
        }
        return false;
    }
}