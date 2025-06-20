namespace PlataformaEducacao.GestaoConteudos.Aplication.Queries.ViewModels;

public class MaterialViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}