using MediatR;
using PlataformaEducacao.GestaoConteudos.Aplication.Commands;
using PlataformaEducacao.GestaoConteudos.Aplication.Events;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Handlers;

public class ProgressoEventHandler(IMediator mediator) : INotificationHandler<AulaConcluidaEvent>
{
    public async Task Handle(AulaConcluidaEvent notification, CancellationToken cancellationToken)
    {
        await mediator.Send(new AtualizarProgressoCursoCommand(notification.CursoId, notification.AlunoId), cancellationToken);
    }
}