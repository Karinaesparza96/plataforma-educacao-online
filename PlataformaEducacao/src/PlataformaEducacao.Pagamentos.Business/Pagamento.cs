using PlataformaEducacao.Core.DomainObjects;

namespace PlataformaEducacao.Pagamentos.Business;

public class Pagamento : Entity, IAggregateRoot
{
    public Guid CursoId { get; set; }
    public Guid AlunoId { get; set; }
    public decimal Valor { get; set; }

    public string NomeCartao { get; set; }
    public string NumeroCartao { get; set; }
    public string ExpiracaoCartao { get; set; }
    public string CvvCartao { get; set; }

    public Transacao Transacao { get; set; }
}