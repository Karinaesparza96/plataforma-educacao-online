using MediatR;
using PlataformaEducacao.Core.Messages;
using PlataformaEducacao.Core.Messages.Notifications;
using PlataformaEducacao.GestaoConteudos.Aplication.Commands;
using PlataformaEducacao.GestaoConteudos.Aplication.Events;
using PlataformaEducacao.GestaoConteudos.Domain;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Handlers;

public class AulaCommandHandler(IMediator mediator, 
                                ICursoRepository cursoRepository,
                                IAulaRepository aulaRepository) : CommandHandler,
                                IRequestHandler<AdicionarAulaCommand, bool>,
                                IRequestHandler<RealizarAulaCommand, bool>,
                                IRequestHandler<ConcluirAulaCommand,bool>
{
    public async Task<bool> Handle(AdicionarAulaCommand request, CancellationToken cancellationToken)
    {
        if (!ValidarComando(request)) 
            return false;

        var curso = await cursoRepository.ObterPorId(request.CursoId);

        if (curso is null)
        {
            await AdicionarNotificacao(request.MessageType, "Curso não encontrado.", cancellationToken);
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

        var aula = await cursoRepository.ObterAulaPorId(request.AulaId);

        if (aula is null)
        {
            await AdicionarNotificacao(request.MessageType, "Aula não encontrada.", cancellationToken);
            return false;
        }

        var progressoAula = await aulaRepository.ObterProgressoAula(aula.Id, request.AlunoId);

        if (progressoAula is null)
        {
            progressoAula = new ProgressoAula(request.AlunoId, request.AulaId);
            progressoAula.EmAndamento();
            aulaRepository.AdicionarProgressoAula(progressoAula);
        }
        else
        {
            progressoAula.EmAndamento();
            aulaRepository.AtualizarProgressoAula(progressoAula);
        }

        return await aulaRepository.UnitOfWork.Commit();
    }

    public async Task<bool> Handle(ConcluirAulaCommand request, CancellationToken cancellationToken)
    {
        if (!ValidarComando(request))
            return false;

        var aula = await cursoRepository.ObterAulaPorId(request.AulaId);

        if (aula is null)
        {
            await AdicionarNotificacao(request.MessageType, "Aula não encontrada.", cancellationToken);
            return false;
        }

        var progressoAula = await aulaRepository.ObterProgressoAula(aula.Id, request.AlunoId);

        if (progressoAula is null)
        {
            await AdicionarNotificacao(request.MessageType, "Progresso não encontrado.", cancellationToken);
            return false;
        }

        progressoAula.ConcluirAula();
        aulaRepository.AtualizarProgressoAula(progressoAula);

        progressoAula.AdicionarEvento(new AulaConcluidaEvent(aula.Id, request.AlunoId, aula.CursoId));

        return await aulaRepository.UnitOfWork.Commit();
    }

    protected override async Task AdicionarNotificacao(string messageType, string descricao, CancellationToken cancellationToken)
    {
       await mediator.Publish(new DomainNotification(messageType, descricao), cancellationToken);
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