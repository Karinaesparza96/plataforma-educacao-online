using Microsoft.EntityFrameworkCore;
using PlataformaEducacao.Core.Data;
using PlataformaEducacao.GestaoConteudos.Data.Context;
using PlataformaEducacao.GestaoConteudos.Domain;

namespace PlataformaEducacao.GestaoConteudos.Data.Repositories;

public class CursoRepository(GestaoConteudosContext dbContext) : ICursoRepository
{
    private readonly DbSet<Curso> _dbSet = dbContext.Set<Curso>();
    public IUnitOfWork UnitOfWork => dbContext;
    
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

    public async Task<IEnumerable<Curso>> ObterTodos()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<Aula?> ObterAulaPorId(Guid aulaId)
    {
        return await dbContext.Set<Aula>().AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == aulaId);
    }

    public async Task<ProgressoCurso?> ObterProgressoCurso(Guid cursoId, Guid alunoId)
    {
       return await dbContext.Set<ProgressoCurso>().AsNoTracking()
            .FirstOrDefaultAsync(pc => pc.CursoId == cursoId && pc.AlunoId == alunoId);
    }

    public void Adicionar(Aula aula)
    {
        dbContext.Set<Aula>().Add(aula);
    }

    public void Adicionar(ProgressoCurso progressoCurso)
    {
        dbContext.Set<ProgressoCurso>().Add(progressoCurso);
    }

    public void Atualizar(ProgressoCurso progressoCurso)
    {
        dbContext.Set<ProgressoCurso>().Update(progressoCurso);
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