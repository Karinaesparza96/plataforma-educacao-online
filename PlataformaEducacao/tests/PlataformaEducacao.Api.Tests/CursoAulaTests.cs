using System.Net.Http.Json;
using PlataformaEducacao.Api.DTOs;
using PlataformaEducacao.Api.Tests.Config;
using PlataformaEducacao.Core.DomainObjects.Enums;

namespace PlataformaEducacao.Api.Tests;

[Collection(nameof(IntegrationApiTestsFixtureCollection))]
public class CursoAulaTests
{
    private readonly IntegrationTestsFixture _fixture;
    public CursoAulaTests(IntegrationTestsFixture fixture)
    {
        _fixture = fixture;
    }
    [Fact(DisplayName = "Cadastro Curso Com Suceso")]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task Adicionar_NovoCurso_DeveExecutarComSucesso()
    {
        // Arrange
        var data = new CursoDto
        {
            Nome = "Curso .NET Core",
            Conteudo = "Curso de .NET Core",
            Preco = 1000,
        };

        await _fixture.RealizarLoginApi();
        _fixture.Client.AtribuirToken(_fixture.Token);

        // Act
        var response = await _fixture.Client.PostAsJsonAsync("/api/cursos", data);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(!string.IsNullOrEmpty(result));
    }
    [Fact(DisplayName = "Cadastro Curso Com Erro")]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task Adicionar_NovoCursoDadosInvalidos_DeveRetornarMensagensErro()
    {
        // Arrange
        var data = new CursoDto
        {
            Nome = "",
            Conteudo = "",
            Preco = 0,
        };

        await _fixture.RealizarLoginApi();
        _fixture.Client.AtribuirToken(_fixture.Token);

        // Act
        var response = await _fixture.Client.PostAsJsonAsync("/api/cursos", data);

        var erros = _fixture.ObterErros(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Contains("O nome do curso não pode ser vazio.", erros.ToString());
        Assert.Contains("O conteúdo programático não pode ser vazio.", erros.ToString());
        Assert.Contains("O preço do curso deve ser maior que zero.", erros.ToString());
    }

    [Fact(DisplayName = "Cadastrar Aula Com Sucesso")]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task Adicionar_NovaAula_DeveExecutarComSucesso()
    {
        // Arrange
        var aula = new AulaDto
        {
            Nome = "Aula 1",
            Conteudo = "Conteudo da Aula 1",
        };

        await _fixture.RealizarLoginApi();
        _fixture.Client.AtribuirToken(_fixture.Token);

        await _fixture.ObterIdsPorStatusMatricula(EStatusMatricula.Ativa);

        // Act
        var response = await _fixture.Client.PostAsJsonAsync($"/api/cursos/{_fixture.CursoId}/aulas/adicionar-aula", aula);

        await response.Content.ReadAsStringAsync();

        // Assert
        response.EnsureSuccessStatusCode();
    }
    [Fact(DisplayName = "Cadastrar Aula Com Erro")]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task Adicionar_NovaAulaDadosInvalidos_DeveRetornarMensagensErro()
    {
        // Arrange
        var aula = new AulaDto
        {
            Nome = "",
            Conteudo = "",
        };

       var idCurso = Guid.NewGuid();

        await _fixture.RealizarLoginApi();
        _fixture.Client.AtribuirToken(_fixture.Token);

        // Act
        var response = await _fixture.Client.PostAsJsonAsync($"/api/cursos/{idCurso}/aulas/adicionar-aula", aula);
        var erros = _fixture.ObterErros(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Contains("O campo Nome não pode ser vazio.", erros.ToString());
        Assert.Contains("O campo Conteudo não pode ser vazio.", erros.ToString());
    }

    [Fact(DisplayName = "Realizar Pagamento com Sucesso")]
    [Trait("Categoria", "Integração Api - Pagamento")]
    public async Task RealizarPagamento_MatriculaAguardandoPagamento_DeveExecutarComSucesso()
    {
        // Arrange
        await _fixture.RealizarLoginApi("aluno@teste.com", "Teste@123");
        _fixture.Client.AtribuirToken(_fixture.Token);

        await _fixture.ObterIdsPorStatusMatricula(EStatusMatricula.AguardandoPagamento);
        _fixture.GerarDadosCartao();

        // Act
        var response = await _fixture.Client.PostAsJsonAsync($"/api/cursos/{_fixture.CursoId}/realizar-pagamento", _fixture.DadosPagamento);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact(DisplayName = "Realizar Aula com Sucesso")]
    [Trait("Categoria", "Integração Api - Aula")]
    public async Task Realizar_MatriculaAtiva_DeveExecutarComSucesso()
    {
        // Arrange
        await _fixture.RealizarLoginApi("aluno@teste.com", "Teste@123");
        _fixture.Client.AtribuirToken(_fixture.Token);

        await _fixture.ObterIdsAulaSemProgresso(EStatusMatricula.Ativa);

        // Act
        var response = await _fixture.Client.PostAsync($"/api/cursos/{_fixture.CursoId}/aulas/{_fixture.AulaId}/realizar-aula", null);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact(DisplayName = "Concluir Aula com Sucesso")]
    [Trait("Categoria", "Integração Api - Aula")]
    public async Task Concluir_MatriculaAtiva_DeveExecutarComSucesso()
    {
        // Arrange
        await _fixture.RealizarLoginApi("aluno@teste.com", "Teste@123");
        _fixture.Client.AtribuirToken(_fixture.Token);

        await _fixture.ObterIdsAulaComProgresso(EStatusMatricula.Ativa);

        // Act
        var response = await _fixture.Client.PostAsync($"/api/cursos/{_fixture.CursoId}/aulas/{_fixture.AulaId}/concluir-aula", null);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact(DisplayName = "Finalizar Curso com Sucesso")]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task ConcluirCurso_AulasConcluidas_DeveExecutarComSucesso()
    {
        // Arrange
        await _fixture.RealizarLoginApi("aluno@teste.com", "Teste@123");
        _fixture.Client.AtribuirToken(_fixture.Token);

        await _fixture.ObterIdCursoAssociadoAulasConcluidas();

        // Act
        var response = await _fixture.Client.PostAsync($"/api/cursos/{_fixture.CursoId}/concluir-curso", null);

        // Assert
        response.EnsureSuccessStatusCode();
    }
    [Fact(DisplayName = "Atualizar Curso com Sucesso")]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task Atualizar_CursoExistente_DeveExecutarComSucesso()
    {
        // Arrange
        await _fixture.RealizarLoginApi();
        _fixture.Client.AtribuirToken(_fixture.Token);
        var id = await _fixture.ObterIdCurso();
        var data = new CursoDto
        {
            Id = id,
            Nome = "Curso .NET Core Atualizado",
            Conteudo = "Curso de .NET Core Atualizado",
            Preco = 2000,
        };
        // Act
        var response = await _fixture.Client.PutAsJsonAsync($"/api/cursos/{id}", data);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        // Assert
        Assert.True(!string.IsNullOrEmpty(result));
    }
    [Fact(DisplayName = "Atualizar Curso com Erro")]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task Atualizar_CursoInexistente_DeveRetornarMensagensErro()
    {
        // Arrange
        await _fixture.RealizarLoginApi();
        _fixture.Client.AtribuirToken(_fixture.Token);
        var id = Guid.NewGuid();
        var data = new CursoDto
        {
            Id = id,
            Nome = "Curso .NET Core Atualizado",
            Conteudo = "Curso de .NET Core Atualizado",
            Preco = 2000,
        };
        // Act
        var response = await _fixture.Client.PutAsJsonAsync($"/api/cursos/{id}", data);
        var erros = _fixture.ObterErros(await response.Content.ReadAsStringAsync());
        // Assert
        Assert.Contains("Curso não encontrado.", erros.ToString());
    }

    [Fact(DisplayName = "Excluir Curso com Sucesso")]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task Deletar_CursoExistente_DeveExecutarComSucesso()
    {
        // Arrange
        await _fixture.RealizarLoginApi();
        _fixture.Client.AtribuirToken(_fixture.Token);
        await _fixture.ObterCursoIdSemAulas();
        // Act
        var response = await _fixture.Client.DeleteAsync($"/api/cursos/{_fixture.CursoId}");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        // Assert
        Assert.True(!string.IsNullOrEmpty(result));
    }
    [Fact(DisplayName = "Excluir Curso com Erro")]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task Deletar_CursoInexistente_DeveRetornarMensagensErro()
    {
        // Arrange
        await _fixture.RealizarLoginApi();
        _fixture.Client.AtribuirToken(_fixture.Token);
        var id = Guid.NewGuid();
        // Act
        var response = await _fixture.Client.DeleteAsync($"/api/cursos/{id}");
        var erros = _fixture.ObterErros(await response.Content.ReadAsStringAsync());
        // Assert
        Assert.Contains("Curso não encontrado.", erros.ToString());
    }
    [Fact(DisplayName = "Excluir Curso com Erro")]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task Deletar_CursoComAulas_DeveRetornarMensagensErro()
    {
        // Arrange
        await _fixture.RealizarLoginApi();
        _fixture.Client.AtribuirToken(_fixture.Token);
        await _fixture.ObterIdsPorStatusMatricula(EStatusMatricula.Ativa);
        // Act
        var response = await _fixture.Client.DeleteAsync($"/api/cursos/{_fixture.CursoId}");
        var erros = _fixture.ObterErros(await response.Content.ReadAsStringAsync());
        // Assert
        Assert.Contains("Curso não pode ser excluído pois possui aulas associadas.", erros.ToString());
    }
}