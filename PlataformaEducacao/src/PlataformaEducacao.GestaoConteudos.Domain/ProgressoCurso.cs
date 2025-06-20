using PlataformaEducacao.Core.DomainObjects;

namespace PlataformaEducacao.GestaoConteudos.Domain;

public class ProgressoCurso : Entity
{
    public Guid CursoId { get; private set; }
    public Guid AlunoId { get; private set; }
    public int TotalAulas { get; private set; }
    public int AulasConcluidas { get; private set; }
    public decimal PercentualConcluido { get; private set; }
    public bool CursoConcluido => PercentualConcluido == 100m;

    protected ProgressoCurso() { }

    public ProgressoCurso(Guid cursoId, Guid alunoId, int totalAulas)
    {
        CursoId = cursoId;
        AlunoId = alunoId;
        TotalAulas = totalAulas;
        AulasConcluidas = 0;
        PercentualConcluido = 0m;
    }

    public void IncrementarProgresso()
    {
        if (AulasConcluidas < TotalAulas)
            AulasConcluidas++;

        AtualizarPercentual();
    }

    private void AtualizarPercentual()
    {
        PercentualConcluido = TotalAulas == 0 ? 0m : Math.Round((decimal)AulasConcluidas / TotalAulas * 100, 2);
    }
}