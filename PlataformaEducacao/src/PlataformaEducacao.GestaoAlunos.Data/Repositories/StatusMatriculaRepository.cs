using Microsoft.EntityFrameworkCore;
using PlataformaEducacao.Core.Data;
using PlataformaEducacao.GestaoAlunos.Data.Context;
using PlataformaEducacao.GestaoAlunos.Domain;

namespace PlataformaEducacao.GestaoAlunos.Data.Repositories;

public class StatusMatriculaRepository(GestaoAlunosContext dbContext) : IStatusMatriculaRepository
{
    private readonly DbSet<StatusMatricula> _dbSet = dbContext.Set<StatusMatricula>();
    public IUnitOfWork UnitOfWork => dbContext;
    public async Task<StatusMatricula?> ObterPorCodigo(int codigo)
    { 
        return  await _dbSet
            .FirstOrDefaultAsync(s => s.Codigo == codigo);
    }

    public void Dispose()
    {
        dbContext.Dispose();
    }
}