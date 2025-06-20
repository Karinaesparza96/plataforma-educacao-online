namespace PlataformaEducacao.GestaoConteudos.Aplication.Queries.ViewModels;

public class AulaViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Conteudo { get; set; } = string.Empty;
    public IEnumerable<MaterialViewModel> Materiais { get; set; } = new List<MaterialViewModel>();
    public string Status { get; set; } = string.Empty;
}