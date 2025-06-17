using MediatR;
using PlataformaEducacao.Core.DomainObjects.DTOs;
using PlataformaEducacao.Core.Messages;
using PlataformaEducacao.Core.Messages.Notifications;
using PlataformaEducacao.Pagamentos.Business.Commands;

namespace PlataformaEducacao.Pagamentos.Business.Handlers;

public class PagamentoCommandHandler(IPagamentoService pagamentoService, IMediator mediator): CommandHandler, IRequestHandler<RealizarPagamentoCursoCommand, bool>
{
    public async Task<bool> Handle(RealizarPagamentoCursoCommand command, CancellationToken cancellationToken)
    {
        if (!ValidarComando(command))
            return false;

        var pagamentoCurso = new PagamentoCurso
        {
            AlunoId = command.AlunoId,
            CursoId = command.CursoId,
            CvvCartao = command.CvvCartao,
            ExpiracaoCartao = command.ExpiracaoCartao,
            NomeCartao = command.NomeCartao,
            NumeroCartao = command.NumeroCartao,
            Total = command.Total
        };
        
        return await pagamentoService.RealizarPagamentoCurso(pagamentoCurso);
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