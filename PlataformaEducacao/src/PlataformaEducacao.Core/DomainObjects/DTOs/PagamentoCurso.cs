namespace PlataformaEducacao.Core.DomainObjects.DTOs;

public class PagamentoCurso
{   
    public Guid CursoId { get; set; }
    public Guid AlunoId { get; set; }
    public decimal Total { get; set; }
    public string NomeCartao { get; set; }
    public string NumeroCartao { get; set; }
    public string ExpiracaoCartao { get; set; }
    public string CvvCartao { get; set; }
}