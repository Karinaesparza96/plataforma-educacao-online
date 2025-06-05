using MediatR;
using PlataformaEducacao.Core.DomainObjects.Enums;
using PlataformaEducacao.Core.Messages;
using PlataformaEducacao.Core.Messages.IntegrationEvents;
using PlataformaEducacao.Core.Messages.IntegrationQueries;
using PlataformaEducacao.Core.Messages.Notifications;
using PlataformaEducacao.GestaoAlunos.Aplication.Commands;
using PlataformaEducacao.GestaoAlunos.Domain;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Handlers;

public class MatriculaCommandHandler(IMediator mediator, 
                                    IAlunoRepository alunoRepository) : CommandHandler,
                                    IRequestHandler<AdicionarMatriculaCommand, bool>,
                                    IRequestHandler<ConcluirMatriculaCommand, bool>,
                                    IRequestHandler<AtivarMatriculaCommand, bool>
{
    public async Task<bool> Handle(AdicionarMatriculaCommand request, CancellationToken cancellationToken)
    {
        if (!ValidarComando(request))
            return false;

        var curso = await mediator.Send(new ObterCursoQuery(request.CursoId), cancellationToken);
        if (curso == null)
        {
            await AdicionarNotificacao(request.MessageType, "Curso não encontrado.", cancellationToken);
            return false;
        }

        var aluno = await alunoRepository.ObterPorId(request.AlunoId);
        if (aluno == null)
        {
            await AdicionarNotificacao(request.MessageType, "Aluno não encontrado.", cancellationToken);
            return false;
        }

        var matriculaExiste = await alunoRepository.ObterMatriculaPorCursoEAlunoId(request.CursoId, aluno.Id);
        if (matriculaExiste != null)
        {
            await AdicionarNotificacao(request.MessageType, "Matrícula já existente.", cancellationToken);
            return false;
        }

        var matricula = new Matricula(request.AlunoId, request.CursoId);

        aluno.AdicionarMatricula(matricula);
        alunoRepository.AdicionarMatricula(matricula);

        return await alunoRepository.UnitOfWork.Commit();
    }

    public async Task<bool> Handle(ConcluirMatriculaCommand request, CancellationToken cancellationToken)
    {
        if (!ValidarComando(request))
            return false;

        var aulas = await mediator.Send(new ObterAulasCursoAlunoQuery(request.CursoId, request.AlunoId), cancellationToken);
        if (!aulas.Aulas.Any())
        {
            await AdicionarNotificacao(request.MessageType, "Aulas não encontradas.", cancellationToken);
            return false;
        }

        var todasAulasConcluidas = aulas.Aulas.All(a => a.Status == EProgressoAulaStatus.Concluida);
        if (!todasAulasConcluidas)
        {
            await AdicionarNotificacao(request.MessageType, "Todas as aulas deste curso precisam estar concluídas.", cancellationToken);
            return false;
        }

        var matricula = await alunoRepository.ObterMatriculaPorCursoEAlunoId(request.CursoId, request.AlunoId);
        if (matricula == null)
        {
            await AdicionarNotificacao(request.MessageType, "Matrícula não encontrada.", cancellationToken);
            return false;
        }
        matricula.Concluir();
        alunoRepository.AtualizarMatricula(matricula);

        matricula.AdicionarEvento(new MatriculaConcluidaEvent(request.AlunoId, matricula.Id, matricula.CursoId));

        return await alunoRepository.UnitOfWork.Commit();
    }

    public async Task<bool> Handle(AtivarMatriculaCommand request, CancellationToken cancellationToken)
    {
        if (!ValidarComando(request))
            return false;

        var matricula = await alunoRepository.ObterMatriculaPorCursoEAlunoId(request.CursoId, request.AlunoId);
        if (matricula == null)
        {
           await AdicionarNotificacao(request.MessageType, "Matrícula não encontrada.", cancellationToken);
            return false;
        }
        matricula.Ativar();
        alunoRepository.AtualizarMatricula(matricula);

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
            mediator.Publish(new DomainNotification(command.MessageType, erro.ErrorMessage));
        }
        return false;
    }
}
