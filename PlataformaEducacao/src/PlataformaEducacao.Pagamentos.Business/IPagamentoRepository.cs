using PlataformaEducacao.Core.Data;

namespace PlataformaEducacao.Pagamentos.Business;

public interface IPagamentoRepository : IRepository<Pagamento>
{
    void Adicionar(Pagamento pagamento);
    void AdicionarTransacao(Transacao transacao);
}