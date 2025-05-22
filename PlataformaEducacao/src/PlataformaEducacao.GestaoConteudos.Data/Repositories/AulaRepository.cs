using Microsoft.EntityFrameworkCore;
using PlataformaEducacao.Core.Data;
using PlataformaEducacao.GestaoConteudos.Data.Context;
using PlataformaEducacao.GestaoConteudos.Domain;

namespace PlataformaEducacao.GestaoConteudos.Data.Repositories;

public class AulaRepository(GestaoConteudosContext dbContext) : IAulaRepository
{
    private readonly DbSet<Aula> _dbSet = dbContext.Set<Aula>();
    public IUnitOfWork UnitOfWork => dbContext;

    public void AdicionarProgressoAula(ProgressoAula progressoAula)
    {
        dbContext.Set<ProgressoAula>().Add(progressoAula);
    }
    public async Task<ProgressoAula?> ObterProgressoAula(Guid aulaId, Guid alunoId)
    {
        return await dbContext.Set<ProgressoAula>().AsNoTracking()
            .FirstOrDefaultAsync(p => p.AulaId == aulaId && p.AlunoId == alunoId);
    }

    public void Dispose()
    {
        dbContext?.Dispose();
    }
}