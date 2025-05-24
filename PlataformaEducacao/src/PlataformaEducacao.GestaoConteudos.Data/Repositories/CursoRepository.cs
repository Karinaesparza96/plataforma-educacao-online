using Microsoft.EntityFrameworkCore;
using PlataformaEducacao.Core.Data;
using PlataformaEducacao.GestaoConteudos.Data.Context;
using PlataformaEducacao.GestaoConteudos.Domain;

namespace PlataformaEducacao.GestaoConteudos.Data.Repositories;

public class CursoRepository(GestaoConteudosContext dbContext) : ICursoRepository
{
    private readonly DbSet<Curso> _dbSet = dbContext.Set<Curso>();
    public IUnitOfWork UnitOfWork => dbContext;

    public async Task<Curso?> ObterCursoPorAulaId(Guid aulaId)
    {
        return await _dbSet.AsNoTracking()
            .Include(c => c.Aulas)
            .FirstOrDefaultAsync(c => c.Aulas.Any(a => a.Id == aulaId));
    }

    public async Task<Curso?> ObterCursoComAulas(Guid cursoId)
    {
        return await _dbSet.AsNoTracking()
            .Include(c => c.Aulas)
            .FirstOrDefaultAsync(c => c.Id == cursoId);
    }

    public async Task<Curso?> ObterPorId(Guid id)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Aula>> ObterAulasComProgressoFiltradoAluno(Guid cursoId, Guid alunoId)
    {
        var aulas = await dbContext.Set<Aula>()
            .Include(a => a.ProgressoAulas)
            .Where(a => a.CursoId == cursoId)
            .AsNoTracking()
            .ToListAsync();

        foreach (var aula in aulas)
        {
            aula.FiltrarProgressoAulaPorAlunoId(alunoId);
        }
        return aulas;
    }
    public async Task<IEnumerable<Curso>> ObterTodos()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<Aula?> ObterAulaPorId(Guid aulaId)
    {
        return await dbContext.Set<Aula>().AsNoTracking()
            .Include(a => a.ProgressoAulas)
            .FirstOrDefaultAsync(a => a.Id == aulaId);
    }

    public void Adicionar(Aula aula)
    {
        dbContext.Set<Aula>().Add(aula);
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
    public void Dispose()
    {
        dbContext.Dispose();
    }
}