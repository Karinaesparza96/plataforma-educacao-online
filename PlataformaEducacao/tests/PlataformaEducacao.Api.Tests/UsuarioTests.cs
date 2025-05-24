using System.Net.Http.Json;
using PlataformaEducacao.Api.DTOs;
using PlataformaEducacao.Api.Tests.Config;

namespace PlataformaEducacao.Api.Tests;

[TestCaseOrderer("PlataformaEducacao.Api.Tests.Config.PriorityOrderer", "PlataformaEducacao.Api.Tests")]
[Collection(nameof(IntegrationApiTestsFixtureCollection))]
public class UsuarioTests
{
    private readonly IntegrationTestsFixture _fixture;

    public UsuarioTests(IntegrationTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "Realizar Login Com Sucesso"), TestPriority(2)]
    [Trait("Categoria", "Integração Api - Aluno")]
    public async Task Aluno_RealizarLogin_DeveExecutarComSucesso()
    {
        // Arrange
        var data = new LoginUserDto()
        {
            Email = _fixture.EmailUsuario,
            Senha = _fixture.SenhaUsuario,
        };
        // Act
        var response = await _fixture.Client.PostAsJsonAsync("/api/conta/login", data);
        response.EnsureSuccessStatusCode();

        _fixture.SalvarUserToken(await response.Content.ReadAsStringAsync());
        // Assert
        Assert.True(!string.IsNullOrEmpty(_fixture.Token));
    }

    [Fact(DisplayName = "Realizar Login Com Sucesso")]
    [Trait("Categoria", "Integração Api - Admin")]
    public async Task Admin_RealizarLogin_DeveExecutarComSucesso()
    {
        // Arrange
        var data = new LoginUserDto()
        {
            Email = _fixture.EmailUsuario,
            Senha = _fixture.SenhaUsuario,
        };

        // Act
       var response = await _fixture.Client.PostAsJsonAsync("/api/conta/login", data);
        response.EnsureSuccessStatusCode();

        _fixture.SalvarUserToken(await response.Content.ReadAsStringAsync());
        // Assert
        Assert.True(!string.IsNullOrEmpty(_fixture.Token));
    }

    [Fact(DisplayName = "Realizar Cadastro Com Sucesso"), TestPriority(1)]
    [Trait("Categoria", "Integração Api - Aluno")]
    public async Task Aluno_RealizarCadastro_DeveExecutarComSucesso()
    {
        // Arrange
        _fixture.GerarDadosUsuario();
        var register = new RegisterUserDto
        {
            Email = _fixture.EmailUsuario,
            Nome = _fixture.EmailUsuario,
            Senha = _fixture.SenhaUsuario,
            ConfirmacaoSenha = _fixture.SenhaConfirmacao
        };

        // Act
        var response = await _fixture.Client.PostAsJsonAsync("/api/conta/registrar/aluno", register);
        response.EnsureSuccessStatusCode();

        _fixture.SalvarUserToken(await response.Content.ReadAsStringAsync());
        // Assert
        Assert.True(!string.IsNullOrEmpty(_fixture.Token));
    }

    [Fact(DisplayName = "Realizar Cadastro Com Sucesso")]
    [Trait("Categoria", "Integração Api - Admin")]
    public async Task Admin_RealizarCadastro_DeveExecutarComSucesso()
    {
        // Arrange
        _fixture.GerarDadosUsuario();
        var register = new RegisterUserDto
        {
            Email = _fixture.EmailUsuario,
            Nome = _fixture.EmailUsuario,
            Senha = _fixture.SenhaUsuario,
            ConfirmacaoSenha = _fixture.SenhaConfirmacao
        };

        // Act
        var response = await _fixture.Client.PostAsJsonAsync("/api/conta/registrar/admin", register);
        response.EnsureSuccessStatusCode();

        _fixture.SalvarUserToken(await response.Content.ReadAsStringAsync());
        // Assert
        Assert.True(!string.IsNullOrEmpty(_fixture.Token));
    }

    [Fact(DisplayName = "Gerar Certificado Com Sucesso")]
    [Trait("Categoria", "Integração Api - Certificado")]
    public async Task BaixarCertificado_AlunoConcluiuCurso_DeveExecutarComSucesso()
    {
        // Arrange
        await _fixture.RealizarLoginApi("aluno@teste.com", "Teste@123");
        _fixture.Client.AtribuirToken(_fixture.Token);

        await _fixture.ObterIdCertificado();
        // Act
        var response = await _fixture.Client.GetAsync($"/api/alunos/certificados/{_fixture.CertificadoId}/download");
        response.EnsureSuccessStatusCode();

        // Assert
        var file = await response.Content.ReadAsByteArrayAsync();
        var filePath = Path.Combine("C:\\Temp", "certificado_teste_integracao.pdf");

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        File.WriteAllBytes(filePath, file);
        Assert.True(File.Exists(filePath));
    }
}