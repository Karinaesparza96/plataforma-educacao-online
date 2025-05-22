using PlataformaEducacao.Core.DomainObjects;
using PlataformaEducacao.Core.DomainObjects.Enums;

namespace PlataformaEducacao.GestaoAlunos.Domain.Tests;

public class AlunoTests
{
    [Fact(DisplayName = "Aluno - Adicionar Matricula")]
    [Trait("Categoria", "GestaoAlunos - AdicionarMatricula")]
    public void AdicionarMatricula_NovaMatricula_StatusAguardandoPagamento()
    {
        // Arrange
        var aluno = new Aluno("teste");
        var cursoId = Guid.NewGuid();
        var matricula = new Matricula(aluno.Id, cursoId);

        // Act
        aluno.AdicionarMatricula(matricula);

        // Assert
        Assert.Equal(1, aluno.Matriculas.Count);
        Assert.Equal(EStatusMatricula.AguardandoPagamento, matricula.Status);
    }
    [Fact(DisplayName = "Aluno - Adicionar Matricula Existente")]
    [Trait("Categoria", "GestaoAlunos - AdicionarMatricula")]
    public void AdicionarMatricula_ExistenteMatricula_DeveLancarException()
    {
        // Arrange
        var aluno = new Aluno("teste");
        var cursoId = Guid.NewGuid();
        var matricula = new Matricula(aluno.Id, cursoId);

        // Act
        aluno.AdicionarMatricula(matricula);

        // Assert
        Assert.Throws<DomainException>(() => aluno.AdicionarMatricula(matricula));
        Assert.Equal(1, aluno.Matriculas.Count);
    }

    [Fact(DisplayName = "Aluno - Adicionar Certificado")]
    [Trait("Categoria", "GestaoAlunos - AdicionarCertificado")]
    public void AdicionarCertificado_NovoCertificado_DeveAdicionarComSucesso()
    {
        // Arrange
        var aluno = new Aluno("teste");
        var curso = "Curso teste";
        var certificado = new Certificado(aluno.Nome, curso, Guid.NewGuid(), aluno.Id, DateTime.Now);

        // Act
        aluno.AdicionarCertificado(certificado);

        // Assert
        Assert.Equal(1, aluno.Certificados.Count);
    }

    [Fact(DisplayName = "Aluno - Gerar Descrição Certificado")]
    [Trait("Categoria", "GestaoAlunos - GerarDescricao")]
    public void GerarDescricao_DadosValidos_DeveGerarDescricao()
    {
        // Arrange
        var aluno = new Aluno("teste");
        var curso = "Curso teste";
        var certificado = new Certificado(aluno.Nome, curso, Guid.NewGuid(), aluno.Id, DateTime.Now);

        // Act
        aluno.AdicionarCertificado(certificado);

        // Assert
        Assert.Contains(aluno.Nome, certificado.Descricao);
        Assert.Contains(curso, certificado.Descricao);
    }

    [Fact(DisplayName = "Aluno - Adicionar Certificado Invalido")]
    [Trait("Categoria", "GestaoAlunos - AdicionarCertificado")]
    public void AdicionarCertificado_DadosInvalidos_DeveLancarException()
    {
        // Arrange && Act && Assert
        Assert.Throws<DomainException>(() => new Certificado("", "", Guid.Empty, Guid.Empty, DateTime.Now));
    }

    [Fact(DisplayName = "Aluno - Adicionar Certificado Existente")]
    [Trait("Categoria", "GestaoAlunos - AdicionarCertificado")]
    public void AdicionarCertificado_CertificadoExistente_DeveLancarException()
    {
        // Arrange
        var aluno = new Aluno("teste");
        var curso = "Curso teste";
        var certificado = new Certificado(aluno.Nome, curso, Guid.NewGuid(), aluno.Id, DateTime.Now);

        // Act
        aluno.AdicionarCertificado(certificado);

        // Assert
        Assert.Throws<DomainException>(() => aluno.AdicionarCertificado(certificado));
        Assert.Equal(1, aluno.Certificados.Count);
    }
    [Fact(DisplayName = "Aluno - Adicionar Certificado Sem Arquivo")]
    [Trait("Categoria", "GestaoAlunos - AdicionarCertificado")]
    public void AdicionarCertificado_SemArquivo_DeveLancarException()
    {
        // Arrange
        var aluno = new Aluno("teste");
        var curso = "Curso teste";
        var certificado = new Certificado(aluno.Nome, curso, Guid.NewGuid(), aluno.Id, DateTime.Now);
        // Act && Assert
        Assert.Throws<DomainException>(() => certificado.AdicionarArquivo(null));
    }
}