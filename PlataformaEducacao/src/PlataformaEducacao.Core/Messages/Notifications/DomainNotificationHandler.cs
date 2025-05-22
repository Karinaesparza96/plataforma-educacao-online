using MediatR;

namespace PlataformaEducacao.Core.Messages;

public class DomainNotificationHandler : INotificationHandler<DomainNotification>
{
    private List<DomainNotification> _notificacoes;

    public DomainNotificationHandler()
    {
        _notificacoes = [];
    }
    public Task Handle(DomainNotification notification, CancellationToken cancellationToken)
    {
        _notificacoes.Add(notification);
        return Task.CompletedTask;
    }

    public virtual List<DomainNotification> ObterNotificacoes()
    {
        return _notificacoes;
    }

    public virtual bool TemNotificacao()
    {
        return ObterNotificacoes().Any();
    }

    public void Dispose()
    {
        _notificacoes = [];
    }
}