using PlataformaEducacao.Core.DomainObjects;

namespace PlataformaEducacao.GestaoConteudos.Domain;

public class Aula(string nome, string conteudo) : Entity, IAggregateRoot
{
    public string Nome { get; private set; } = nome;
    public string Conteudo { get; private set; } = conteudo;
    public Guid CursoId { get; private set; }

    private readonly List<Material> _materiais = [];
    public IReadOnlyCollection<Material> Materiais => _materiais;

    // EF relationship
    public Curso? Curso { get; private set; }

    public void AssociarCurso(Guid cursoId)
    {
        CursoId = cursoId;
    }

    public void AdicionarMaterial(Material material)
    {
        if (MaterialExistente(material))
            throw new DomainException("Material já associado a esta aula.");

        material.AssociarAula(Id);
        _materiais.Add(material);
    }

    private bool MaterialExistente(Material material)
    {
        return _materiais.Any(m => m.Id == material.Id);
    }
}