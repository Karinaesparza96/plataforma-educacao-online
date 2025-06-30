namespace PlataformaEducacao.GestaoConteudos.Aplication.Queries.ViewModels;

public class HistoricoAprendizagemCursoViewModel
{
    public Guid CursoId { get; set; }
    public string NomeCurso { get; set; }
    public int TotalAulas { get; set; }
    public int AulasConcluidas { get; set; }
    public decimal PercentualConcluido { get; set; }
    public List<HistoricoAprendizagemAulaViewModel> Aulas { get; set; } = [];
}

public class HistoricoAprendizagemAulaViewModel
{
    public Guid AulaId { get; set; }
    public string NomeAula { get; set; }
    public string Status { get; set; }
}