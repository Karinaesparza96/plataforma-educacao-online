using Microsoft.EntityFrameworkCore;
using PlataformaEducacao.Core.Data;
using PlataformaEducacao.Pagamentos.Business;
using PlataformaEducacao.Pagamentos.Data.Context;

namespace PlataformaEducacao.Pagamentos.Data.Repository;

public class PagamentoRepository(PagamentoContext context) : IPagamentoRepository
{
    private readonly DbSet<Pagamento> _dbSet = context.Set<Pagamento>();
    public IUnitOfWork UnitOfWork => context;
    public void Adicionar(Pagamento pagamento)
    {
        _dbSet.Add(pagamento);
    }

    public void AdicionarTransacao(Transacao transacao)
    {
        context.Set<Transacao>().Add(transacao);
    }

    public void Dispose()
    {
       context.Dispose();
    }
}