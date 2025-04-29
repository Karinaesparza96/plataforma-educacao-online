using NetDevPack.SimpleMediator;

namespace PlataformaEducacao.Core.Messages;

public class DomainNotificationHandler : INotificationHandler<DomainNotification>
{
    private List<DomainNotification> _notificacoes;

    public DomainNotificationHandler()
    {
        _notificacoes = new List<DomainNotification>();
    }
    public Task Handle(DomainNotification notification, CancellationToken cancellationToken)
    {
        _notificacoes.Add(notification);
        return Task.CompletedTask;
    }
}