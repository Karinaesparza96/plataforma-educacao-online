using PlataformaEducacao.Core.Messages;

namespace PlataformaEducacao.Core.DomainObjects;

public abstract class Entity
{
    public Guid Id { get; protected set; }

    private List<Event> _notificacoes;
    public IReadOnlyCollection<Event>? Notificacoes => _notificacoes?.AsReadOnly();
    public DateTime? DataCriacao { get; set; }
    public DateTime? DataAlteracao { get; set; }
    public DateTime? DataExclusao { get; set; }

    protected Entity()
    {
        Id = Guid.NewGuid();
    }

    protected Entity(Guid id)
    {
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity entity) 
            return false;

        if (ReferenceEquals(this, entity)) 
            return true;

        return Id.Equals(entity.Id);
    }

    public static bool operator ==(Entity a, Entity b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) 
            return true;

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity a, Entity b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return (GetType().GetHashCode() * 907) + Id.GetHashCode();
    }

    public void AdicionarEvento(Event evento)
    {
        _notificacoes ??= [];
        _notificacoes.Add(evento);
    }

    public override string ToString()
    {
        return $"{GetType().Name} - [Id={Id}]";
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