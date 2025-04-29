using PlataformaEducacao.Core.Messages;

namespace PlataformaEducacao.Core.DomainObjects;

public abstract class Entity
{
    public Guid Id { get; protected set; }

    private List<Event> _notificacoes;
    public IReadOnlyCollection<Event>? Notificacoes => _notificacoes?.AsReadOnly();
    protected Entity()
    {
        Id = Guid.NewGuid();
    }

    public void AdicionarEvento(Event evento)
    {
        _notificacoes ??= [];
        _notificacoes.Add(evento);
    }

    public void RemoverEvento(Event evento)
    {
        _notificacoes?.Remove(evento);
    }

    public void LimparEventos()
    {
        _notificacoes?.Clear();
    }
}