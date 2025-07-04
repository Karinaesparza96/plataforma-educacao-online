﻿using MediatR;
using PlataformaEducacao.Core.DomainObjects.Enums;
using PlataformaEducacao.Core.Messages;
using PlataformaEducacao.Core.Messages.IntegrationEvents;
using PlataformaEducacao.Core.Messages.Notifications;
using PlataformaEducacao.GestaoAlunos.Aplication.Commands;
using PlataformaEducacao.GestaoAlunos.Domain;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Handlers;

public class MatriculaCommandHandler(IMediator mediator,
                                    IStatusMatriculaRepository statusMatriculaRepository,
                                    IAlunoRepository alunoRepository) : CommandHandler,
                                    IRequestHandler<AdicionarMatriculaCommand, bool>,
                                    IRequestHandler<ConcluirMatriculaCommand, bool>,
                                    IRequestHandler<AtivarMatriculaCommand, bool>
{
    public async Task<bool> Handle(AdicionarMatriculaCommand request, CancellationToken cancellationToken)
    {
        if (!ValidarComando(request))
            return false;

        var aluno = await alunoRepository.ObterPorId(request.AlunoId);
        if (aluno is null)
        {
            await AdicionarNotificacao(request.MessageType, "Aluno não encontrado.", cancellationToken);
            return false;
        }

        var matriculaExiste = await alunoRepository.ObterMatriculaPorCursoEAlunoId(request.CursoId, aluno.Id);
        if (matriculaExiste is not null)
        {
            await AdicionarNotificacao(request.MessageType, "Matrícula já existente.", cancellationToken);
            return false;
        }
        var status = await statusMatriculaRepository.ObterPorCodigo((int)EStatusMatricula.Iniciada);
        var matricula = new Matricula(request.AlunoId, request.CursoId, status);

        var statusAguardandoPag = await statusMatriculaRepository.ObterPorCodigo((int)EStatusMatricula.AguardandoPagamento);
        matricula.AguardandoPagamento(statusAguardandoPag);

        aluno.AdicionarMatricula(matricula);
        alunoRepository.AdicionarMatricula(matricula);

        return await alunoRepository.UnitOfWork.Commit();
    }

    public async Task<bool> Handle(ConcluirMatriculaCommand request, CancellationToken cancellationToken)
    {
        if (!ValidarComando(request))
            return false;

        var matricula = await alunoRepository.ObterMatriculaPorCursoEAlunoId(request.CursoId, request.AlunoId);
        if (matricula is null)
        {
            await AdicionarNotificacao(request.MessageType, "Matrícula não encontrada.", cancellationToken);
            return false;
        }
        var status = await statusMatriculaRepository.ObterPorCodigo((int)EStatusMatricula.Concluida);
        matricula.Concluir(status);

        alunoRepository.AtualizarMatricula(matricula);

        matricula.AdicionarEvento(new MatriculaConcluidaEvent(request.AlunoId, matricula.Id, request.CursoId, request.NomeCurso));

        return await alunoRepository.UnitOfWork.Commit();
    }

    public async Task<bool> Handle(AtivarMatriculaCommand request, CancellationToken cancellationToken)
    {
        if (!ValidarComando(request))
            return false;

        var matricula = await alunoRepository.ObterMatriculaPorCursoEAlunoId(request.CursoId, request.AlunoId);
        if (matricula is null)
        {
           await AdicionarNotificacao(request.MessageType, "Matrícula não encontrada.", cancellationToken);
            return false;
        }
        var status = await statusMatriculaRepository.ObterPorCodigo((int)EStatusMatricula.Ativa);
        matricula.Ativar(status);

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
