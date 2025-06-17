namespace PlataformaEducacao.GestaoConteudos.Aplication.Queries.ViewModels;

public class CursoViewModel
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Nome { get; set; } = string.Empty;
    public string ConteudoProgramatico { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public IEnumerable<AulaViewModel> Aulas { get; set; } = new List<AulaViewModel>();
}
public class AulaViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Conteudo { get; set; } = string.Empty;
    public IEnumerable<MaterialViewModel> Materiais { get; set; } = new List<MaterialViewModel>();
    public string Status { get; set; } = string.Empty;
}
public class MaterialViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}
