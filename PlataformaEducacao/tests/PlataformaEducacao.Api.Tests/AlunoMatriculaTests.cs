using PlataformaEducacao.Api.Tests.Config;

namespace PlataformaEducacao.Api.Tests;

[Collection(nameof(IntegrationApiTestsFixtureCollection))]
public class AlunoMatriculaTests
{
    private readonly IntegrationTestsFixture _fixture;
    public AlunoMatriculaTests(IntegrationTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "Realizar Matrícula com Sucesso")]
    [Trait("Categoria", "Integração Api - Matricula")]
    public async Task Adicionar_NovaMatricula_DeveExecutarComSucesso()
    {
        // Arrange
        await _fixture.RealizarRegistroAluno();
        _fixture.Client.AtribuirToken(_fixture.Token);

        var curso = await _fixture.ObterIdCurso();

        // Act
        var response = await _fixture.Client.PostAsync($"/api/matriculas/{curso}", null);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact(DisplayName = "Realizar Matrícula com Erro")]
    [Trait("Categoria", "Integração Api - Matricula")]
    public async Task Adicionar_NovaMatricula_DeveRetornarMensagensErro()
    {
        // Arrange
        await _fixture.RealizarRegistroAluno();
        _fixture.Client.AtribuirToken(_fixture.Token);

        var curso = Guid.NewGuid();

        // Act
        var response = await _fixture.Client.PostAsync($"/api/matriculas/{curso}", null);
        var erros = _fixture.ObterErros(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Contains("Curso não encontrado.", erros.ToString());
    }
}