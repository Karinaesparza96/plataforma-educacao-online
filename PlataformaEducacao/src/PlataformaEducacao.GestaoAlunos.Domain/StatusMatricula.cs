using PlataformaEducacao.Core.DomainObjects;

namespace PlataformaEducacao.GestaoAlunos.Domain;

public class StatusMatricula : Entity, IAggregateRoot
{
    public int Codigo { get; set; }
    public string Descricao { get; set; }
}