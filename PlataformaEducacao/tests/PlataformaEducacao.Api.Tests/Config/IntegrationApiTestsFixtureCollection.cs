using Bogus;
using Bogus.DataSets;
using Dapper;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlataformaEducacao.Api.DTOs;
using PlataformaEducacao.Core.DomainObjects.Enums;
using System.Text;
using System.Text.Json;

namespace PlataformaEducacao.Api.Tests.Config;

[CollectionDefinition(nameof(IntegrationApiTestsFixtureCollection))]
public class IntegrationApiTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture> { }

public class IntegrationTestsFixture : IDisposable
{   
    public readonly PlataformaEducacaoAppFactory Factory;
    public HttpClient Client;
    public string ConnectionString { get; set; }
    public string NomeUsuario { get; set; }
    public string SenhaUsuario { get; set; }
    public string EmailUsuario { get; set; }
    public string SenhaConfirmacao { get; set; }
    public string Token { get; set; }
    public DadosPagamento DadosPagamento { get; set; }
    public Guid CursoId { get; set; }
    public Guid MatriculaId { get; set; }
    public Guid AlunoId { get; set; }
    public Guid AulaId { get; set; }
    public Guid CertificadoId { get; set; }

    public IntegrationTestsFixture()
    {
        var options = new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("http://localhost:5224")
        };
        Factory = new PlataformaEducacaoAppFactory();
        Client = Factory.CreateClient(options);
        DadosPagamento = new DadosPagamento();
        var configuration = Factory.Services.GetRequiredService<IConfiguration>();
        ConnectionString = configuration.GetConnectionString("DefaultConnection") ?? 
                           throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
    }

    public void GerarDadosUsuario()
    {
        var faker = new Faker("pt_BR");
        EmailUsuario = faker.Internet.Email().ToLower();
        NomeUsuario = EmailUsuario;
        SenhaUsuario = faker.Internet.Password(8, false, "", "@1Ab_");
        SenhaConfirmacao = SenhaUsuario;
    }

    public void GerarDadosCartao()
    {
        var faker = new Faker("pt_BR");
        DadosPagamento.NomeCartao = faker.Name.FullName();
        DadosPagamento.NumeroCartao = faker.Finance.CreditCardNumber(CardType.Mastercard);
        DadosPagamento.ExpiracaoCartao = faker.Date.Future(1, DateTime.Now).ToString("MM/yy");
        DadosPagamento.CvvCartao = faker.Finance.CreditCardCvv();
    }

    public async Task ObterIdsPorStatusMatricula(EStatusMatricula status)
    {
        var sql = @"
                select
	                m.Id matriculaId,
	                c.Id cursoId,
	                a.Id alunoId,
	                al.id aulaId
                from
	                Matriculas m
                join Cursos c on
	                c.Id = m.CursoId
                join Alunos a on
	                a.Id = m.AlunoId
                join Aulas al on al.CursoId = c.Id
                where
	                1=1
                and m.Status = @Status
            ";

        await ExecutarConsulta(sql, new { Status = status }, (retorno) =>
        {
            if (retorno != null)
            {
                MatriculaId = Guid.Parse(retorno.matriculaId);
                CursoId = Guid.Parse(retorno.cursoId);
                AlunoId = Guid.Parse(retorno.alunoId);
                AulaId = Guid.Parse(retorno.aulaId);
            }

            return retorno;
        });
    }

    public async Task ObterIdsAulaComProgresso(EStatusMatricula status)
    {
        var sql = @"
                select
	                m.Id matriculaId,
	                c.Id cursoId,
	                a.Id alunoId,
	                al.id aulaId
                from
	                Matriculas m
                join Cursos c on
	                c.Id = m.CursoId
                join Alunos a on
	                a.Id = m.AlunoId
                join Aulas al on al.CursoId = c.Id
                join ProgressoAulas pa on pa.AulaId = al.Id and pa.AlunoId = a.Id  
                where
	                1=1
                and m.Status = @Status
            ";

        await ExecutarConsulta(sql, new { Status = status }, (retorno) =>
        {
            if (retorno != null)
            {
                MatriculaId = Guid.Parse(retorno.matriculaId);
                CursoId = Guid.Parse(retorno.cursoId);
                AlunoId = Guid.Parse(retorno.alunoId);
                AulaId = Guid.Parse(retorno.aulaId);
            }

            return retorno;
        });
    }

    public async Task ObterIdsAulaSemProgresso(EStatusMatricula status)
    {
        var sql = @"
                     select
                         m.Id matriculaId,
                         c.Id cursoId,
                         a.Id alunoId,
                         al.id aulaId,
                         pa.id
                     from
                         Matriculas m
                     join Cursos c on
                         c.Id = m.CursoId
                     join Alunos a on
                         a.Id = m.AlunoId
                     join Aulas al on al.CursoId = c.Id
                     left join ProgressoAulas pa on pa.AulaId = al.Id and pa.AlunoId = a.Id
                     where
                         1=1
                     and m.Status = @Status
                     and pa.id is null
                ";

        await ExecutarConsulta(sql, new { Status = status }, (retorno) =>
        {
            if (retorno != null)
            {
                MatriculaId = Guid.Parse(retorno.matriculaId);
                CursoId = Guid.Parse(retorno.cursoId);
                AlunoId = Guid.Parse(retorno.alunoId);
                AulaId = Guid.Parse(retorno.aulaId);
            }
            return retorno;
        });
    }

    public async Task ObterIdCursoAssociadoAulasConcluidas()
    {
        var sql = @"
                    select
	                    distinct c.Id
                    from
	                    cursos c
                    join aulas a on
	                    a.CursoId = c.Id
                    join ProgressoAulas pa on
	                    pa.AulaId = a.Id
                    join Alunos al on
	                    al.Id = pa.AlunoId
                    join AspNetUsers anu on
	                    al.Id = UPPER(anu.Id)
                    where
	                    1=1
	                    and pa.Status = 2";

        await ExecutarConsulta(sql, param: null, (retorno) =>
        {
            if (retorno != null)
            {
                CursoId = Guid.Parse(retorno.Id);
            }
            return retorno;
        });
    }

    public async Task<HttpResponseMessage> PostAsync(string url, object? data = null)
    {
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await Client.PostAsync(url, content);
    }

    public async Task<HttpResponseMessage> PutAsync(string url, object data)
    {
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await Client.PutAsync(url, content);
    }

    public void SalvarUserToken(string token)
    {
       var response = JsonSerializer.Deserialize<LoginResponseWrapper>(token,
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) ?? new LoginResponseWrapper();
       Token = response.Data.AccessToken;
       AlunoId = Guid.Parse(response.Data.UserToken.Id);
    }

    public async Task RealizarLoginApi(string? email = null, string? senha = null)
    {
        var userData = new LoginUserDto()
        {
            Email = email ?? "admin@teste.com",
            Senha = senha ?? "Teste@123"
        };
        Client = Factory.CreateClient();

        var response = await PostAsync("/api/conta/login", userData);
        response.EnsureSuccessStatusCode();

        SalvarUserToken(await response.Content.ReadAsStringAsync());
    }
    
    public async Task RealizarRegistroAluno()
    {
        GerarDadosUsuario();
        var registerUserDto = new RegisterUserDto()
        {
            Nome = NomeUsuario,
            Email = EmailUsuario,
            Senha = SenhaUsuario,
            ConfirmacaoSenha = SenhaConfirmacao
        };
        Client = Factory.CreateClient();

        var response = await PostAsync("/api/conta/registrar/aluno", registerUserDto);
        response.EnsureSuccessStatusCode();

        SalvarUserToken(await response.Content.ReadAsStringAsync());
    }

    public async Task<Guid> ObterIdCurso()
    {
        var response = await Client.GetAsync("/api/cursos");
        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadAsStringAsync();

        var json = JsonSerializer.Deserialize<JsonElement>(data);
        return json.GetProperty("data")[0].GetProperty("id").GetGuid();
    }

    public async Task ObterCursoIdSemAulas()
    {
        var sql = @"
                    select c.Id from Cursos c 
                    left join Aulas a on a.CursoId = c.id
                    where a.Id is null 
                   ";
        await ExecutarConsulta(sql, param: null, (retorno) =>
        {
            if (retorno != null)
            {
                CursoId = Guid.Parse(retorno.Id);
            }
            return retorno;
        });
    }

    public async Task ObterIdCertificado()
    { 
         var sql = @"select c.Id from Certificados c";

         await ExecutarConsulta(sql, param: null, (retorno) =>
         {
             if (retorno != null)
             {
                 CertificadoId = Guid.Parse(retorno.Id);
             }
             return retorno;
         });
    }

    public JsonElement ObterErros(string result)
    {
        var json = JsonSerializer.Deserialize<JsonElement>(result);
        return json.GetProperty("erros");
    }
    public void Dispose()
    {
        Factory.Dispose();
        Client.Dispose();
    }

    private async Task<TResult?> ExecutarConsulta<TResult>(string sql, object? param, Func<dynamic, TResult?> processar)
    {
        await using var connection = new SqliteConnection(ConnectionString);
        await connection.OpenAsync();

        var result = await connection.QueryFirstOrDefaultAsync<TResult>(sql, param);

        await connection.CloseAsync();
        return processar(result);
    }
}