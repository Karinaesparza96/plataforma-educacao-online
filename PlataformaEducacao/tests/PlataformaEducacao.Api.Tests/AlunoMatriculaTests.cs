using PlataformaEducacao.Api.Tests.Config;

namespace PlataformaEducacao.Api.Tests;

[Collection(nameof(IntegrationApiTestsFixtureCollection))]
public class AlunoMatriculaTests
{
    private readonly IntegrationTestsFixture _integrationTestsFixture;
    public AlunoMatriculaTests(IntegrationTestsFixture integrationTestsFixture)
    {
        _integrationTestsFixture = integrationTestsFixture;
    }

    [Fact(DisplayName = "Realizar Matrícula com Sucesso")]
    [Trait("Categoria", "Integração Api - Matricula")]
    public async Task Adicionar_NovaMatricula_DeveExecutarComSucesso()
    {
        // Arrange
        await _integrationTestsFixture.RealizarRegistroAluno();
        _integrationTestsFixture.Client.AtribuirToken(_integrationTestsFixture.Token);
        var curso = await _integrationTestsFixture.ObterIdCurso();

        // Act
        var response = await _integrationTestsFixture.PostAsync($"/api/matriculas/{curso}");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact(DisplayName = "Realizar Matrícula com Erro")]
    [Trait("Categoria", "Integração Api - Matricula")]
    public async Task Adicionar_NovaMatricula_DeveRetornarMensagensErro()
    {
        // Arrange
        await _integrationTestsFixture.RealizarRegistroAluno();
        _integrationTestsFixture.Client.AtribuirToken(_integrationTestsFixture.Token);
        var curso = Guid.NewGuid();

        // Act
        var response = await _integrationTestsFixture.PostAsync($"/api/matriculas/{curso}");
        var erros = _integrationTestsFixture.ObterErros(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Contains("Curso não encontrado.", erros.ToString());
    }
}