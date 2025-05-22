namespace PlataformaEducacao.Core.DomainObjects;

public class Usuario : Entity, IAggregateRoot
{
    public void AssociarUsuario(string IdIdentity)
    {
        Id = Guid.Parse(IdIdentity);
    }
}