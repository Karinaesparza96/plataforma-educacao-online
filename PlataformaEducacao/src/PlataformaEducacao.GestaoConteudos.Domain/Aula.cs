using PlataformaEducacao.Core.DomainObjects;

namespace PlataformaEducacao.GestaoConteudos.Domain;

public class Aula : Entity, IAggregateRoot
{
    public string Nome { get; private set; }
    public string Conteudo { get; private set; }
    public Guid CursoId { get; private set; }

    private readonly List<Material> _materiais = [];
    public IReadOnlyCollection<Material> Materiais => _materiais;

    private readonly List<ProgressoAula> _progressoAulas = [];
    public IReadOnlyCollection<ProgressoAula> ProgressoAulas => _progressoAulas;

    // EF relationship
    public Curso? Curso { get; private set; }

    // Ef Constructor
    protected Aula() {}

    public Aula(string nome, string conteudo)
    {
        Nome = nome;
        Conteudo = conteudo;
        Validar();
    }

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

        progressoAula.EmAndamento();
        _progressoAulas.Add(progressoAula);
    }

    public void ConcluirAula(ProgressoAula progressoAula)
    {
        if (!ProgressoAulaExistente(progressoAula))
            throw new DomainException("Progresso não encontrado para esta aula.");

        progressoAula.ConcluirAula();
    }

    public void FiltrarProgressoAulaPorAlunoId(Guid alunoId)
    {
        var filtrados = _progressoAulas.Where(p => p.AlunoId == alunoId).ToList();
        _progressoAulas.Clear();
        _progressoAulas.AddRange(filtrados);
    }

    private void Validar()
    {
        if (string.IsNullOrWhiteSpace(Nome))
            throw new DomainException("O nome da aula é obrigatório.");

        if (string.IsNullOrWhiteSpace(Conteudo))
            throw new DomainException("O conteúdo da aula é obrigatório.");
    }

    private bool MaterialExistente(Material material)
    {
        return _materiais.Any(m => m.Id == material.Id);
    }
    private bool ProgressoAulaExistente(ProgressoAula progressoAula)
    {
        return _progressoAulas.Any(p => p.AlunoId == progressoAula.AlunoId && p.AulaId == progressoAula.AulaId);
    }
}