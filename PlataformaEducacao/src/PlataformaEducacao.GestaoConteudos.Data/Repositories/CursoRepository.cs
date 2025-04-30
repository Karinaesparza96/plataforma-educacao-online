using Microsoft.EntityFrameworkCore;
using PlataformaEducacao.Core.Data;
using PlataformaEducacao.GestaoConteudos.Data.Context;
using PlataformaEducacao.GestaoConteudos.Domain;

namespace PlataformaEducacao.GestaoConteudos.Data.Repositories;

public class CursoRepository : ICursoRepository
{
    private readonly GestaoConteudosContext _dbContext;
    private readonly DbSet<Curso> _dbSet;
    public IUnitOfWork UnitOfWork => _dbContext;
    public CursoRepository(GestaoConteudosContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<Curso>();
    }

    public async Task<Curso?> ObterCursoPorAulaId(Guid aulaId)
    {
        return await _dbSet.AsNoTracking()
            .Include(c => c.Aulas)
            .FirstOrDefaultAsync(c => c.Aulas.Any(a => a.Id == aulaId));
    }

    public async Task<Curso?> ObterPorId(Guid id)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
    }
    public async Task<IEnumerable<Curso>> ObterTodos()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<Aula?> ObterAulaPorId(Guid aulaId)
    {
        return await _dbContext.Set<Aula>().AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == aulaId);
    }

    public void Adicionar(Aula aula)
    {
        _dbContext.Set<Aula>().Add(aula);
    }

    public void Adicionar(Curso curso)
    {
        _dbSet.Add(curso);
    }
    public void Atualizar(Curso curso)
    {
        _dbSet.Update(curso);
    }
    public void Remover(Curso curso)
    {
        _dbSet.Remove(curso);
    }
    public async Task<bool> Commit()
    {
        return await UnitOfWork.Commit();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}