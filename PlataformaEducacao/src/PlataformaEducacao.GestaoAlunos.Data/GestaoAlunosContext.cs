using Microsoft.EntityFrameworkCore;
using PlataformaEducacao.Core.Data;
using PlataformaEducacao.GestaoAlunos.Domain;

namespace PlataformaEducacao.GestaoAlunos.Data;

public class GestaoAlunosContext(DbContextOptions<GestaoAlunosContext> options) : DbContext(options), IUnitOfWork
{   
    public DbSet<Matricula> Matriculas { get; set; }

    public Task<bool> Commit()
    {
        throw new NotImplementedException();
    }
}