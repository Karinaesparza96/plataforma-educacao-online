using MediatR;
using PlataformaEducacao.Core.Messages.IntegrationEvents;
using PlataformaEducacao.GestaoAlunos.Aplication.Commands;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Handlers;

public class MatriculaEventHandler(IMediator mediator) 
    : INotificationHandler<CursoPagamentoRealizadoEvent>, 
      INotificationHandler<MatriculaConcluidaEvent>
{
    public async Task Handle(CursoPagamentoRealizadoEvent notification, CancellationToken cancellationToken)
    {
        await mediator.Send(new AtivarMatriculaCommand(notification.AlunoId, notification.CursoId), cancellationToken);
    }

    public async Task Handle(MatriculaConcluidaEvent notification, CancellationToken cancellationToken)
    {
        await mediator.Send(new AdicionarCertificadoCommand(notification.AlunoId, notification.MatriculaId, notification.CursoId), cancellationToken);
    }
}