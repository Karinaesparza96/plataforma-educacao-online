using PlataformaEducacao.Core.DomainObjects;

namespace PlataformaEducacao.GestaoConteudos.Domain;

public class Material : Entity
{
    public string Nome { get; private set; }
    public string Tipo { get; private set; }
    public Guid AulaId { get; private set; }

    // EF relationship
    public Aula Aula { get; private set; }

    public Material(string nome, string tipo)
    {
        Nome = nome;
        Tipo = tipo;
    }

    public void AssociarAula(Guid aulaId)
    {
        AulaId = aulaId;
    }
}