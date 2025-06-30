namespace PlataformaEducacao.Api.DTOs;

public class CursoDto
{   
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Conteudo { get; set; }
    public decimal Preco { get; set; }
}

public class CursoNovoDto
{
    public string Nome { get; set; }
    public string Conteudo { get; set; }
    public decimal Preco { get; set; }
}