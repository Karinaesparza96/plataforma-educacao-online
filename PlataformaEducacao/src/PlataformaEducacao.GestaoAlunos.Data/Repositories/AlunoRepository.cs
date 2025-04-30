using Microsoft.EntityFrameworkCore;
using PlataformaEducacao.Core.Data;
using PlataformaEducacao.GestaoAlunos.Domain;

namespace PlataformaEducacao.GestaoAlunos.Data.Repositories;

public class AlunoRepository : IAlunoRepository
{
    private readonly GestaoAlunosContext _dbContext;
    private readonly DbSet<Aluno> _dbSet;
    public IUnitOfWork UnitOfWork => _dbContext;
    public AlunoRepository(GestaoAlunosContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<Aluno>();
    }
    public async Task<Aluno?> ObterPorId(Guid alunoId)
    {
        return await _dbSet.FindAsync(alunoId);
    }

    public async Task<Matricula?> ObterMatriculaCursoPorAlunoId(Guid alunoId, Guid cursoId)
    {
        return await _dbContext.Matriculas.AsNoTracking()
            .FirstOrDefaultAsync(m => m.AlunoId == alunoId && m.CursoId == cursoId);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
  
}