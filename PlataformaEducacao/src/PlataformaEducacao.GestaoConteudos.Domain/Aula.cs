using PlataformaEducacao.Core.DomainObjects;

namespace PlataformaEducacao.GestaoConteudos.Domain;

public class Aula(string nome, string conteudo) : Entity, IAggregateRoot
{
    public string Nome { get; private set; } = nome;
    public string Conteudo { get; private set; } = conteudo;
    public Guid CursoId { get; private set; }

    private readonly List<Material> _materiais = [];
    public IReadOnlyCollection<Material> Materiais => _materiais;

    private readonly List<ProgressoAula> _progressoAulas = [];
    public IReadOnlyCollection<ProgressoAula> ProgressoAulas => _progressoAulas;

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

    public void AdicionarProgresso(ProgressoAula progressoAula)
    {
        if (ProgressoAulaExistente(progressoAula))
            throw new DomainException("Progresso já registrado para esta aula.");

        _progressoAulas.Add(progressoAula);
    }

    private bool MaterialExistente(Material material)
    {
        return _materiais.Any(m => m.Id == material.Id);
    }
    private bool ProgressoAulaExistente(ProgressoAula progressoAula)
    {
        return _progressoAulas.Any(p => p.Id == progressoAula.Id);
    }
}