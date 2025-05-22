using MediatR;
using PlataformaEducacao.Core.DomainObjects;
using PlataformaEducacao.Core.DomainObjects.DTOs;
using PlataformaEducacao.Core.DomainObjects.Enums;
using PlataformaEducacao.Core.Messages;
using PlataformaEducacao.Core.Messages.IntegrationQueries;
using PlataformaEducacao.GestaoConteudos.Aplication.Commands;
using PlataformaEducacao.GestaoConteudos.Domain;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Handlers;

public class CursoCommandHandler(ICursoRepository cursoRepository,
                                IPagamentoService pagamentoService,
                                IMediator mediator) 
                                        : IRequestHandler<AdicionarCursoCommand, bool>, IRequestHandler<RealizarPagamentoCursoCommand, bool>
{
    public async Task<bool> Handle(AdicionarCursoCommand command, CancellationToken cancellationToken)
    {
        if (!ValidarComando(command)) return false;

        var curso = new Curso(command.Nome, command.ConteudoProgramatico, command.UsuarioCriacaoId, command.Preco);
        cursoRepository.Adicionar(curso);

        return await cursoRepository.UnitOfWork.Commit();
    }

    public async Task<bool> Handle(RealizarPagamentoCursoCommand command, CancellationToken cancellationToken)
    {
        if (!ValidarComando(command)) 
            return false;

        var curso = await cursoRepository.ObterPorId(command.CursoId);
        if (curso == null)
        {
            await mediator.Publish(new DomainNotification(command.MessageType, "Curso não encontrado."), cancellationToken);
            return false;
        }

        var matricula = await mediator.Send(new ObterMatriculaCursoAlunoQuery(command.CursoId, command.AlunoId), cancellationToken);

        if (matricula is not { Status: EStatusMatricula.AguardandoPagamento })
        {
            await mediator.Publish(new DomainNotification(command.MessageType, "Matrícula não realizada."), cancellationToken);
            return false;
        }

        var pagamentoCurso = new PagamentoCurso
        {
            AlunoId = command.AlunoId,
            CursoId = curso.Id,
            CvvCartao = command.CvvCartao,
            ExpiracaoCartao = command.ExpiracaoCartao,
            NomeCartao = command.NomeCartao,
            NumeroCartao = command.NumeroCartao,
            Total = curso.Preco
        };

        return await pagamentoService.RealizarPagamentoCurso(pagamentoCurso);
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