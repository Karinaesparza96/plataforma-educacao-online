using PlataformaEducacao.Core.DomainObjects;

namespace PlataformaEducacao.Pagamentos.Business;

public class Transacao : Entity
{
    public Guid MatriculaId { get; set; }
    public Guid PagamentoId { get; set; }
    public decimal Total { get; set; }
    public StatusTransacao StatusTransacao { get; set; }

    public Pagamento Pagamento { get; set; }
}