using Microsoft.EntityFrameworkCore;
using PlataformaEducacao.Core.Data;
using PlataformaEducacao.GestaoAlunos.Data.Context;
using PlataformaEducacao.GestaoAlunos.Domain;

namespace PlataformaEducacao.GestaoAlunos.Data.Repositories;

public class UsuarioRepository(GestaoAlunosContext dbContext) : IUsuarioRepository
{
    private readonly DbSet<Usuario> _dbSet = dbContext.Set<Usuario>();
    public IUnitOfWork UnitOfWork => dbContext;
    public void Adicionar(Usuario usuario)
    {
        _dbSet.Add(usuario);
    }

    public void Dispose()
    {
        dbContext.Dispose();
    }
}