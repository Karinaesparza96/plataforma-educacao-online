using System.Net.Http.Json;
using PlataformaEducacao.Api.DTOs;
using PlataformaEducacao.Api.Tests.Config;

namespace PlataformaEducacao.Api.Tests;

[TestCaseOrderer("PlataformaEducacao.Api.Tests.Config.PriorityOrderer", "PlataformaEducacao.Api.Tests")]
[Collection(nameof(IntegrationApiTestsFixtureCollection))]
public class UsuarioTests(IntegrationTestsFixture fixture)
{
    [Fact(DisplayName = "Realizar Login Com Sucesso"), TestPriority(2)]
    [Trait("Categoria", "Integração Api - Aluno")]
    public async Task Aluno_RealizarLogin_DeveExecutarComSucesso()
    {
        // Arrange
        var data = new LoginUserDto()
        {
            Email = fixture.EmailUsuario,
            Senha = fixture.SenhaUsuario,
        };
        // Act
        var response = await fixture.Client.PostAsJsonAsync("/api/conta/login", data);
        response.EnsureSuccessStatusCode();

        fixture.SalvarUserToken(await response.Content.ReadAsStringAsync());
        // Assert
        Assert.True(!string.IsNullOrEmpty(fixture.Token));
    }

    [Fact(DisplayName = "Realizar Login Com Sucesso")]
    [Trait("Categoria", "Integração Api - Admin")]
    public async Task Admin_RealizarLogin_DeveExecutarComSucesso()
    {
        // Arrange
        var data = new LoginUserDto()
        {
            Email = fixture.EmailUsuario,
            Senha = fixture.SenhaUsuario,
        };

        // Act
       var response = await fixture.Client.PostAsJsonAsync("/api/conta/login", data);
        response.EnsureSuccessStatusCode();

        fixture.SalvarUserToken(await response.Content.ReadAsStringAsync());
        // Assert
        Assert.True(!string.IsNullOrEmpty(fixture.Token));
    }

    [Fact(DisplayName = "Realizar Login Com Erro")]
    [Trait("Categoria", "Integração Api - Usuario")]
    public async Task RealizarLogin_UsuarioInexistente_DeveRetornarMensagensDeErro()
    {
        // Arrange
        var data = new LoginUserDto()
        {
            Email = "email@email.com",
            Senha = "Teste@123",
        };

        // Act
        var response = await fixture.Client.PostAsJsonAsync("/api/conta/login", data);

        // Assert
        var erros = fixture.ObterErros(await response.Content.ReadAsStringAsync());
        Assert.Contains("Usuário ou Senha incorretos", erros.ToString());
    }

    [Fact(DisplayName = "Realizar Cadastro Com Sucesso"), TestPriority(1)]
    [Trait("Categoria", "Integração Api - Aluno")]
    public async Task Aluno_RealizarCadastro_DeveExecutarComSucesso()
    {
        // Arrange
        fixture.GerarDadosUsuario();
        var register = new RegisterUserDto
        {
            Email = fixture.EmailUsuario,
            Nome = fixture.EmailUsuario,
            Senha = fixture.SenhaUsuario,
            ConfirmacaoSenha = fixture.SenhaConfirmacao
        };

        // Act
        var response = await fixture.Client.PostAsJsonAsync("/api/conta/registrar/aluno", register);
        response.EnsureSuccessStatusCode();

        fixture.SalvarUserToken(await response.Content.ReadAsStringAsync());
        // Assert
        Assert.True(!string.IsNullOrEmpty(fixture.Token));
    }

    [Fact(DisplayName = "Realizar Cadastro Com Sucesso")]
    [Trait("Categoria", "Integração Api - Admin")]
    public async Task Admin_RealizarCadastro_DeveExecutarComSucesso()
    {
        // Arrange
        fixture.GerarDadosUsuario();
        var register = new RegisterUserDto
        {
            Email = fixture.EmailUsuario,
            Nome = fixture.EmailUsuario,
            Senha = fixture.SenhaUsuario,
            ConfirmacaoSenha = fixture.SenhaConfirmacao
        };

        // Act
        var response = await fixture.Client.PostAsJsonAsync("/api/conta/registrar/admin", register);
        response.EnsureSuccessStatusCode();

        fixture.SalvarUserToken(await response.Content.ReadAsStringAsync());
        // Assert
        Assert.True(!string.IsNullOrEmpty(fixture.Token));
    }

    [Fact(DisplayName = "Gerar Certificado Com Sucesso")]
    [Trait("Categoria", "Integração Api - Certificado")]
    public async Task BaixarCertificado_AlunoConcluiuCurso_DeveExecutarComSucesso()
    {
        // Arrange
        await fixture.RealizarLoginApi("aluno@teste.com", "Teste@123");
        fixture.Client.AtribuirToken(fixture.Token);

        await fixture.ObterIdCertificado();
        // Act
        var response = await fixture.Client.GetAsync($"/api/alunos/certificados/{fixture.CertificadoId}/download");
        response.EnsureSuccessStatusCode();

        // Assert
        var file = await response.Content.ReadAsByteArrayAsync();
        var filePath = Path.Combine(Path.GetTempPath(), "certificado_teste_integracao.pdf");

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        File.WriteAllBytes(filePath, file);
        Assert.True(File.Exists(filePath));
    }

    [Fact(DisplayName = "Obter Historico Aprendizagem Aluno - Com Historico")]
    [Trait("Categoria", "Integração Api - Aluno")]
    public async Task ObterHistoricoAprendizagem_AlunoPossuiHistorico_DeveDevolverComSucesso()
    {
        // Arrange
        await fixture.RealizarLoginApi("aluno@teste.com", "Teste@123");
        fixture.Client.AtribuirToken(fixture.Token);

        await fixture.ObterCursoHistoricoAprendizado();
        // Act
        var response = await fixture.Client.GetAsync($"/api/alunos/historico-aprendizagem/{fixture.CursoId}");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact(DisplayName = "Obter Historico Aprendizagem Aluno - Sem Historico")]
    [Trait("Categoria", "Integração Api - Aluno")]
    public async Task ObterHistoricoAprendizagem_AlunoSemHistorico_DeveDevolverComErro()
    {
        // Arrange
        await fixture.RealizarLoginApi("aluno@teste.com", "Teste@123");
        fixture.Client.AtribuirToken(fixture.Token);
        fixture.CursoId = Guid.NewGuid();

        // Act
        var response = await fixture.Client.GetAsync($"/api/alunos/historico-aprendizagem/{fixture.CursoId}");

        // Assert
        Assert.False(response.IsSuccessStatusCode);
    }
}