using System.Net.Http.Json;
using PlataformaEducacao.Api.Tests.Config;
using PlataformaEducacao.Core.DomainObjects.Enums;

namespace PlataformaEducacao.Api.Tests;

[Collection(nameof(IntegrationApiTestsFixtureCollection))]
public class PagamentoTests
{
    private readonly IntegrationTestsFixture _fixture;
    public PagamentoTests(IntegrationTestsFixture fixture)
    {
        _fixture = fixture;
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

    [Fact(DisplayName = "Realizar Pagamento com Erro")]
    [Trait("Categoria", "Integração Api - Pagamento")]
    public async Task RealizarPagamento_MatriculaNaoAtiva_DeveRetornarMensagensErro()
    {
        // Arrange
        await _fixture.RealizarLoginApi("aluno2@teste.com", "Teste@123");
        _fixture.Client.AtribuirToken(_fixture.Token);

        await _fixture.ObterIdsPorStatusMatricula(EStatusMatricula.Ativa);
        _fixture.GerarDadosCartao();

        // Act
        var response = await _fixture.Client.PostAsJsonAsync($"/api/cursos/{_fixture.CursoId}/realizar-pagamento", _fixture.DadosPagamento);


        var erros = _fixture.ObterErros(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Contains("A matrícula deve estar com status 'Aguardando Pagamento' para realizar o pagamento."
            , erros.ToString());
    }
}