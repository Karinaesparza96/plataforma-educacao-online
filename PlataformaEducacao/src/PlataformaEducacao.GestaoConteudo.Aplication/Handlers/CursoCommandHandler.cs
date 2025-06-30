using MediatR;
using PlataformaEducacao.Core.Messages;
using PlataformaEducacao.Core.Messages.Notifications;
using PlataformaEducacao.GestaoConteudos.Aplication.Commands;
using PlataformaEducacao.GestaoConteudos.Domain;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Handlers;

public class CursoCommandHandler(ICursoRepository cursoRepository,
                                 IMediator mediator) : CommandHandler,
                                                    IRequestHandler<AdicionarCursoCommand, bool>, 
                                                    IRequestHandler<AtualizarCursoCommand, bool>,
                                                    IRequestHandler<DeletarCursoCommand, bool>,
                                                    IRequestHandler<AtualizarProgressoCursoCommand, bool>
{
    public async Task<bool> Handle(AdicionarCursoCommand command, CancellationToken cancellationToken)
    {
        if (!ValidarComando(command)) return false;

        var curso = new Curso(command.Nome, command.ConteudoProgramatico, command.UsuarioCriacaoId, command.Preco);
        cursoRepository.Adicionar(curso);

        return await cursoRepository.UnitOfWork.Commit();
    }
    
    public async Task<bool> Handle(AtualizarCursoCommand command, CancellationToken cancellationToken)
    {
        if (!ValidarComando(command))
            return false;

        var curso = await cursoRepository.ObterPorId(command.CursoId);
        if (curso is null)
        {
            await AdicionarNotificacao(command.MessageType, "Curso não encontrado.", cancellationToken);
            return false;
        }

        curso.AtualizarNome(command.Nome);
        curso.AtualizarConteudo(command.ConteudoProgramatico);
        curso.AtualizarPreco(command.Preco);

        cursoRepository.Atualizar(curso);
        return await cursoRepository.UnitOfWork.Commit();
    }
    public async Task<bool> Handle(DeletarCursoCommand command, CancellationToken cancellationToken)
    {
        if (!ValidarComando(command))
            return false;

        var curso = await cursoRepository.ObterCursoComAulas(command.CursoId);
        if (curso is null)
        {
            await AdicionarNotificacao(command.MessageType, "Curso não encontrado.", cancellationToken);
            return false;
        }

        if (curso.Aulas.Any())
        {
            await AdicionarNotificacao(command.MessageType, "Curso não pode ser excluído pois possui aulas associadas.", cancellationToken);
            return false;
        }

        cursoRepository.Remover(curso);
        return await cursoRepository.UnitOfWork.Commit();
    }
    public async Task<bool> Handle(AtualizarProgressoCursoCommand request, CancellationToken cancellationToken)
    {
        if (!ValidarComando(request))
            return false;

        var curso = await cursoRepository.ObterCursoComAulas(request.CursoId);
        if (curso is null)
        {
            await AdicionarNotificacao(request.MessageType, "Curso não encontrado.", cancellationToken);
            return false;
        }

        var progressoCurso = await cursoRepository.ObterProgressoCurso(request.CursoId, request.AlunoId);

        if (progressoCurso is null)
        {
            progressoCurso = new ProgressoCurso(request.CursoId, request.AlunoId, curso.Aulas.Count);
            progressoCurso.IncrementarProgresso();
            cursoRepository.Adicionar(progressoCurso);
        }
        else
        {
            progressoCurso.IncrementarProgresso();
            cursoRepository.Atualizar(progressoCurso);
        }
        return await cursoRepository.UnitOfWork.Commit();
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