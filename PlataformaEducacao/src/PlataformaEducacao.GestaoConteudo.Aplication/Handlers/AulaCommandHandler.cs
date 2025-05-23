using MediatR;
using PlataformaEducacao.Core.DomainObjects.Enums;
using PlataformaEducacao.Core.Messages;
using PlataformaEducacao.Core.Messages.IntegrationQueries;
using PlataformaEducacao.Core.Messages.Notifications;
using PlataformaEducacao.GestaoConteudos.Aplication.Commands;
using PlataformaEducacao.GestaoConteudos.Domain;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Handlers;

public class AulaCommandHandler(IMediator mediator, 
                                ICursoRepository cursoRepository,
                                IAulaRepository aulaRepository) 
    : IRequestHandler<AdicionarAulaCommand, bool>, IRequestHandler<RealizarAulaCommand, bool>
{
    public async Task<bool> Handle(AdicionarAulaCommand request, CancellationToken cancellationToken)
    {
        if (!ValidarComando(request)) 
            return false;

        var curso = await cursoRepository.ObterPorId(request.CursoId);

        if (curso == null)
        {
            await mediator.Publish(new DomainNotification(request.MessageType, "Curso não encontrado."), cancellationToken);
            return false;
        }

        var aula = new Aula(request.Nome, request.Conteudo);
        aula.AssociarCurso(request.CursoId);

        if (request is { NomeMaterial: not null, TipoMaterial: not null })
            aula.AdicionarMaterial(new Material(request.NomeMaterial, request.TipoMaterial));

        cursoRepository.Adicionar(aula);

        return await cursoRepository.UnitOfWork.Commit();
    }

    public async Task<bool> Handle(RealizarAulaCommand request, CancellationToken cancellationToken)
    {
        if (!ValidarComando(request))
            return false;

        if(!await ValidarMatricula(request.CursoId, request.AlunoId, request.MessageType, cancellationToken))
            return false;

        var aula = await cursoRepository.ObterAulaPorId(request.AulaId);

        if (aula == null)
        {
            await mediator.Publish(new DomainNotification(request.MessageType, "Aula não encontrada."), cancellationToken);
            return false;
        }
        var progressoAula = new ProgressoAula(request.AlunoId, request.AulaId);

        aula.AdicionarProgresso(progressoAula);

        aulaRepository.AdicionarProgressoAula(progressoAula);

        return await aulaRepository.UnitOfWork.Commit();
    }

    public async Task<bool> Handle(ConcluirAulaCommand request, CancellationToken cancellationToken)
    {
        if (!ValidarComando(request))
            return false;

        if(!await ValidarMatricula(request.CursoId, request.AlunoId, request.MessageType, cancellationToken))
            return false;

        var aula = await cursoRepository.ObterAulaPorId(request.AulaId);

        if (aula == null)
        {
            await mediator.Publish(new DomainNotification(request.MessageType, "Aula não encontrada."), cancellationToken);
            return false;
        }

        var progressoAula = await aulaRepository.ObterProgressoAula(aula.Id, request.AlunoId);

        if (progressoAula == null)
        {
            await mediator.Publish(new DomainNotification(request.MessageType, "Progresso não encontrado."), cancellationToken);
            return false;
        }

        aula.ConcluirAula(progressoAula);

        aulaRepository.AdicionarProgressoAula(progressoAula);

        return await aulaRepository.UnitOfWork.Commit();
    }
    private async Task<bool> ValidarMatricula(Guid cursoId, Guid alunoId, string messageType, CancellationToken cancellationToken)
    {
        var matricula = await mediator.Send(new ObterMatriculaCursoAlunoQuery(cursoId, alunoId), cancellationToken);
        if (matricula == null)
        {
            await mediator.Publish(new DomainNotification(messageType, "Matrícula não encontrada."), cancellationToken);
            return false;
        }

        if (matricula.Status != EStatusMatricula.Ativa)
        {
            await mediator.Publish(new DomainNotification(messageType, "Matrícula não está ativa."), cancellationToken);
            return false;
        }
        return true;
    }

    private bool ValidarComando(Command command)
    {
        if (command.EhValido()) return true;
        foreach (var erro in command.ValidationResult.Errors)
        {
            mediator.Publish(new DomainNotification(command.MessageType, erro.ErrorMessage));
        }
        return false;
    }
}