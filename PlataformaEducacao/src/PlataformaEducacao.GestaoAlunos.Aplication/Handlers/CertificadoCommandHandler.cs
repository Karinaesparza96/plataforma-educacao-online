using MediatR;
using PlataformaEducacao.Core.Messages;
using PlataformaEducacao.Core.Messages.IntegrationQueries;
using PlataformaEducacao.Core.Messages.Notifications;
using PlataformaEducacao.GestaoAlunos.Aplication.Commands;
using PlataformaEducacao.GestaoAlunos.Domain;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Handlers;

public class CertificadoCommandHandler(ICertificadoPdfService certificadoPdfService,
                                        IMediator mediator,
                                       IAlunoRepository alunoRepository) : CommandHandler,
                                        IRequestHandler<AdicionarCertificadoCommand, bool>
{
    public async Task<bool> Handle(AdicionarCertificadoCommand request, CancellationToken cancellationToken)
    {
        if (!ValidarComando(request))
            return false;

        var aluno = await alunoRepository.ObterPorId(request.AlunoId);
        if (aluno == null)
        {
            await AdicionarNotificacao(request.MessageType, "Aluno não encontrado.", cancellationToken);
            return false;
        }
        var matricula = await alunoRepository.ObterMatriculaPorCursoEAlunoId(request.CursoId, request.AlunoId);
        if (matricula == null)
        {
            await AdicionarNotificacao(request.MessageType, "Matrícula não encontrada.", cancellationToken);
            return false;
        }

        var curso = await mediator.Send(new ObterCursoQuery(request.CursoId), cancellationToken);
        if (curso == null) 
        {
            await AdicionarNotificacao(request.MessageType, "Curso não encontrado.", cancellationToken);
            return false;
        }

        var certificado = new Certificado(aluno.Nome, curso.Nome, matricula.Id, aluno.Id, matricula.DataConclusao);

        var pdf = certificadoPdfService.GerarPdf(certificado);

        if (!pdf.Any())
        {
            await AdicionarNotificacao(request.MessageType, "Erro ao gerar o PDF do certificado.", cancellationToken);
            return false;
        }

        certificado.AdicionarArquivo(pdf);

        aluno.AdicionarCertificado(certificado);
        alunoRepository.AdicionarCertificado(certificado);

        return await alunoRepository.UnitOfWork.Commit();
    }
    protected override async Task AdicionarNotificacao(string messageType, string descricao, CancellationToken cancellationToken)
    {
       await mediator.Publish(new DomainNotification(messageType, descricao), cancellationToken);
    }
    private bool ValidarComando(Command command)
    {
        if (command.EhValido())
            return true;

        foreach (var erro in command.ValidationResult.Errors)
        {
            mediator.Publish(new DomainNotification(command.MessageType, erro.ErrorMessage), CancellationToken.None);
        }
        return false;
    }
}
