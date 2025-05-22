using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlataformaEducacao.Api.Data;
using PlataformaEducacao.Core.DomainObjects;
using PlataformaEducacao.GestaoAlunos.Data.Context;
using PlataformaEducacao.GestaoAlunos.Domain;
using PlataformaEducacao.GestaoConteudos.Data.Context;
using PlataformaEducacao.GestaoConteudos.Domain;
using PlataformaEducacao.Pagamentos.Business;
using PlataformaEducacao.Pagamentos.Data.Context;

namespace PlataformaEducacao.Api.Configurations;

public static class DbMigrationHelpers
{
    public static void UseDbMigrationHelper(this WebApplication app)
    {
        EnsureSeedData(app).Wait();
    }

    public static async Task EnsureSeedData(WebApplication application)
    {
        var service = application.Services.CreateScope().ServiceProvider;
        await EnsureSeedData(service);
    }

    public static async Task EnsureSeedData(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var contextGestaoConteudos = scope.ServiceProvider.GetRequiredService<GestaoConteudosContext>();
        var contextGestaoAlunos = scope.ServiceProvider.GetRequiredService<GestaoAlunosContext>();
        var contextIdentity = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        var contextPagamentos = scope.ServiceProvider.GetRequiredService<PagamentoContext>();
        var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
        var certificadoService = scope.ServiceProvider.GetRequiredService<ICertificadoPdfService>();

        if (env.IsDevelopment() || env.IsEnvironment("Testing"))
        {   
            await contextGestaoAlunos.Database.EnsureDeletedAsync();
            await contextGestaoConteudos.Database.EnsureDeletedAsync();
            await contextIdentity.Database.EnsureDeletedAsync();
            await contextPagamentos.Database.EnsureDeletedAsync();

            await contextGestaoConteudos.Database.MigrateAsync();
            await contextGestaoAlunos.Database.MigrateAsync();
            await contextIdentity.Database.MigrateAsync();
            await contextPagamentos.Database.MigrateAsync();

            await SeedUsersAndRoles(scope.ServiceProvider);
            await SeedDataInitial(contextGestaoAlunos, contextGestaoConteudos, contextIdentity, contextPagamentos, certificadoService);
        }
    }

    private static async Task SeedUsersAndRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        var roles = new List<string>() { "ADMIN", "ALUNO" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var user = new IdentityUser
        {
            Email = "aluno@teste.com",
            EmailConfirmed = true,
            UserName = "aluno@teste.com",
        };

        var userAdmin = new IdentityUser
        {
            Email = "admin@teste.com",
            EmailConfirmed = true,
            UserName = "admin@teste.com",
        };

        await userManager.CreateAsync(user, "Teste@123");
        await userManager.CreateAsync(userAdmin, "Teste@123");

        await userManager.AddToRoleAsync(user, "ALUNO");
        await userManager.AddToRoleAsync(userAdmin, "ADMIN");
    }

    private static async Task SeedDataInitial(GestaoAlunosContext dbAlunosContext, GestaoConteudosContext dbConteudosContext, ApplicationContext dbApplicationContext, PagamentoContext dbPagamentoContext, ICertificadoPdfService certificadoPdfService)
    {
        if (dbAlunosContext.Set<Aluno>().Any() || dbAlunosContext.Set<Matricula>().Any() || dbAlunosContext.Set<Usuario>().Any())
            return;
        if (dbConteudosContext.Set<Curso>().Any() || dbConteudosContext.Set<Aula>().Any())
            return;

        var user = await dbApplicationContext.Users.FirstOrDefaultAsync(x => x.Email == "aluno@teste.com");
        var userAdmin = await dbApplicationContext.Users.FirstOrDefaultAsync(x => x.Email == "admin@teste.com");

        var admin = new Usuario();
        admin.AssociarUsuario(userAdmin.Id);

        var aluno = new Aluno("fulano");
        aluno.AssociarUsuario(user.Id);

        var curso = new Curso("Curso C#", "Teste", admin.Id, 250);
        var aula = new Aula("Aula 1", "Teste");
        var aula2 = new Aula("Aula 2", "Teste");
        var aula3 = new Aula("Aula 3", "Teste");

        var curso2 = new Curso("Angular", "Teste", admin.Id, 150);
        var aula4 = new Aula("Aula 1", "Teste");
        var aula5 = new Aula("Aula 2", "Teste");
        var aula6 = new Aula("Aula 3", "Teste");

        var curso3 = new Curso("EF Core", "Teste", admin.Id, 150);
        var aula7 = new Aula("Aula 1", "Teste");

        curso3.AdicionarAula(aula7);

        curso2.AdicionarAula(aula4);
        curso2.AdicionarAula(aula5);
        curso2.AdicionarAula(aula6);

        curso.AdicionarAula(aula);
        curso.AdicionarAula(aula2);
        curso.AdicionarAula(aula3);

        var matriculaConcluida = new Matricula(aluno.Id, curso.Id);
        matriculaConcluida.Concluir();

        var matriculaAtiva = new Matricula(aluno.Id, curso2.Id);
        matriculaAtiva.Ativar();

        var matriculaAguardando = new Matricula(aluno.Id, curso3.Id);
        matriculaAguardando.AguardarPagamento();

        var progressoAula = new ProgressoAula(aluno.Id, aula.Id);
        var progressoAula2 = new ProgressoAula(aluno.Id, aula2.Id);
        var progressoAula3 = new ProgressoAula(aluno.Id, aula3.Id);

        aula.AdicionarProgresso(progressoAula);
        aula2.AdicionarProgresso(progressoAula2);
        aula3.AdicionarProgresso(progressoAula3);

        aula.ConcluirAula(progressoAula);
        aula2.ConcluirAula(progressoAula2);
        aula3.ConcluirAula(progressoAula3);

        var pagamento = new Pagamento
        {
            AlunoId = aluno.Id,
            CursoId = curso.Id,
            NomeCartao = "Nome do Cartão",
            NumeroCartao = "5502093788528294",
            ExpiracaoCartao = "12/25",
            CvvCartao = "455",
            Valor = curso.Preco,
        };
        var transacao = new Transacao
        {   
            PagamentoId = pagamento.Id,
            MatriculaId = matriculaAtiva.Id,
            StatusTransacao = StatusTransacao.Pago,
            Pagamento = pagamento,
            Total = pagamento.Valor,
        };
        var certificado = new Certificado(aluno.Nome, curso.Nome, matriculaConcluida.Id, aluno.Id, matriculaConcluida.DataConclusao);
        var pdf = certificadoPdfService.GerarPdf(certificado);
        certificado.AdicionarArquivo(pdf);

        await dbAlunosContext.Set<Aluno>().AddAsync(aluno);
        await dbAlunosContext.Set<Usuario>().AddAsync(admin);
        await dbAlunosContext.Set<Matricula>().AddRangeAsync([matriculaConcluida, matriculaAtiva, matriculaAguardando]);
        await dbAlunosContext.Set<Certificado>().AddAsync(certificado);
        await dbConteudosContext.Set<Curso>().AddRangeAsync([curso, curso2, curso3]);
        await dbConteudosContext.Set<Aula>().AddRangeAsync([aula, aula2, aula3]);
        await dbConteudosContext.Set<ProgressoAula>().AddRangeAsync([progressoAula, progressoAula2, progressoAula3]);
        await dbPagamentoContext.Set<Pagamento>().AddAsync(pagamento);
        await dbPagamentoContext.Set<Transacao>().AddAsync(transacao);

        await dbAlunosContext.SaveChangesAsync();
        await dbConteudosContext.SaveChangesAsync();
        await dbPagamentoContext.SaveChangesAsync();
    }

}