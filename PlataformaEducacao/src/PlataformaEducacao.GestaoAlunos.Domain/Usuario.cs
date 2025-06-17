using PlataformaEducacao.Core.DomainObjects;

namespace PlataformaEducacao.GestaoAlunos.Domain;

public class Usuario : Entity, IAggregateRoot
{   
    public Usuario(Guid Id) : base(Id) {}

}