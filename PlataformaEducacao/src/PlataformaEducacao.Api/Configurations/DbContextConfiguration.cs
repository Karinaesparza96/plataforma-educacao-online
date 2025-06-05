using Microsoft.EntityFrameworkCore;
using PlataformaEducacao.Api.Data;
using PlataformaEducacao.GestaoAlunos.Data.Context;
using PlataformaEducacao.GestaoConteudos.Data.Context;
using PlataformaEducacao.Pagamentos.Data.Context;

namespace PlataformaEducacao.Api.Configurations;

public static class DbContextConfiguration
{
    public static WebApplicationBuilder AddDbContextConfiguration(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsProduction())
        {
            builder.Services.AddDbContext<GestaoConteudosContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<GestaoAlunosContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }, ServiceLifetime.Transient);
            builder.Services.AddDbContext<ApplicationContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<PagamentoContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
        }
        else
        {
            builder.Services.AddDbContext<GestaoConteudosContext>(opt =>
            {
                opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<GestaoAlunosContext>(opt =>
            {
                opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
            }, ServiceLifetime.Transient);
            builder.Services.AddDbContext<ApplicationContext>(opt =>
            {
                opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<PagamentoContext>(opt =>
            {
                opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
        }
       
        return builder;
    }
}