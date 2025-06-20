namespace PlataformaEducacao.GestaoConteudos.Aplication.Queries.ViewModels;

public class CursoViewModel
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Nome { get; set; } = string.Empty;
    public string ConteudoProgramatico { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public IEnumerable<AulaViewModel> Aulas { get; set; } = new List<AulaViewModel>();
}