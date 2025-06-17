using MediatR;
using Moq;
using Moq.AutoMock;
using PlataformaEducacao.Core.Messages.Notifications;
using PlataformaEducacao.GestaoAlunos.Aplication.Commands;
using PlataformaEducacao.GestaoAlunos.Aplication.Handlers;
using PlataformaEducacao.GestaoAlunos.Aplication.Services;
using PlataformaEducacao.GestaoAlunos.Domain;
using Matricula = PlataformaEducacao.GestaoAlunos.Domain.Matricula;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Tests;

public class CertificadoCommandHandlerTests
{
    private readonly AutoMocker _mocker;
    private readonly CertificadoCommandHandler _handler;
    private readonly Mock<IAlunoRepository> _alunoRepositoryMock;
    private readonly Mock<ICertificadoPdfService> _certificadoPdfServiceMock;
    private readonly Aluno _aluno;
    private readonly Guid _cursoId;
    private readonly Guid _alunoId;
    private readonly Guid _matriculaId;

    public CertificadoCommandHandlerTests()
    {
        _mocker = new AutoMocker();
        _handler = _mocker.CreateInstance<CertificadoCommandHandler>();
        _alunoRepositoryMock = _mocker.GetMock<IAlunoRepository>();
        _certificadoPdfServiceMock = _mocker.GetMock<ICertificadoPdfService>();
        _aluno = new Aluno(Guid.NewGuid(), "teste");
        _cursoId = Guid.NewGuid();
        _alunoId = Guid.NewGuid();
        _matriculaId = Guid.NewGuid();
    }

    [Fact(DisplayName = "Criar Certificado Com Sucesso")]
    [Trait("Categoria", "GestaoAlunos - CertificadoCommandHandler")]
    public async Task AdicionarCertificado_NovoCertificado_DeveExecutarComSucesso()
    {
        // Arrange
        var command = new AdicionarCertificadoCommand(_alunoId, _matriculaId, _cursoId, "Curso C#");
        var matricula = new Matricula(_alunoId, _cursoId);
        matricula.Concluir();

        _alunoRepositoryMock.Setup(r => r.ObterPorId(command.AlunoId)).ReturnsAsync(_aluno);
        _alunoRepositoryMock.Setup(r => r.ObterMatriculaPorCursoEAlunoId(command.CursoId, command.AlunoId)).ReturnsAsync(matricula);
        _alunoRepositoryMock.Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);
        _certificadoPdfServiceMock.Setup(r => r.GerarPdf(It.IsAny<Certificado>())).Returns(new byte[10]);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _alunoRepositoryMock.Verify(r => r.UnitOfWork.Commit(), Times.Once);
        _alunoRepositoryMock.Verify(r => r.ObterPorId(command.AlunoId), Times.Once);
        _alunoRepositoryMock.Verify(r => r.AdicionarCertificado(It.IsAny<Certificado>()), Times.Once);
    }

    [Fact(DisplayName = "Criar Certificado Command invalido")]
    [Trait("Categoria", "GestaoAlunos - CertificadoCommandHandler")]
    public async Task AdicionarCertificado_CommandInvalido_NaoDeveExecutarComSucesso()
    {
        // Arrange
        var command = new AdicionarCertificadoCommand(Guid.Empty, Guid.Empty, Guid.Empty, "");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _alunoRepositoryMock.Verify(r => r.UnitOfWork.Commit(), Times.Never);
        _alunoRepositoryMock.Verify(r => r.ObterPorId(command.AlunoId), Times.Never);
        _alunoRepositoryMock.Verify(r => r.AdicionarCertificado(It.IsAny<Certificado>()), Times.Never);
        Assert.Contains(GerarCertificadoCommandValidator.CursoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(GerarCertificadoCommandValidator.AlunoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(GerarCertificadoCommandValidator.MatriculaIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(GerarCertificadoCommandValidator.NomeCursoErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Equal(4, command.ValidationResult.Errors.Count);
    }

    [Fact(DisplayName = "Criar Certificado - Aluno não encontrado")]
    [Trait("Categoria", "GestaoAlunos - CertificadoCommandHandler")]
    public async Task AdicionarCertificado_AlunoNaoEncontrado_NaoDeveExecutarComSucesso()
    {
        // Arrange
        var command = new AdicionarCertificadoCommand(_alunoId, _matriculaId, _cursoId, "Curso C#");
        _alunoRepositoryMock.Setup(r => r.ObterPorId(command.AlunoId)).ReturnsAsync((Aluno?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _alunoRepositoryMock.Verify(r => r.UnitOfWork.Commit(), Times.Never);
        _alunoRepositoryMock.Verify(r => r.ObterPorId(command.AlunoId), Times.Once);
        _alunoRepositoryMock.Verify(r => r.AdicionarCertificado(It.IsAny<Certificado>()), Times.Never);
        _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<DomainNotification>(), CancellationToken.None), Times.Once);
    }

    [Fact(DisplayName = "Criar Certificado - Matricula não encontrada")]
    [Trait("Categoria", "GestaoAlunos - CertificadoCommandHandler")]
    public async Task AdicionarCertificado_MatriculaNaoEncontrada_NaoDeveExecutarComSucesso()
    {
        // Arrange
        var command = new AdicionarCertificadoCommand(_alunoId, _matriculaId, _cursoId, "Curso C#");
        _alunoRepositoryMock.Setup(r => r.ObterPorId(command.AlunoId)).ReturnsAsync(_aluno);
        _alunoRepositoryMock.Setup(r => r.ObterMatriculaPorCursoEAlunoId(command.CursoId, command.AlunoId)).ReturnsAsync((Matricula?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _alunoRepositoryMock.Verify(r => r.UnitOfWork.Commit(), Times.Never);
        _alunoRepositoryMock.Verify(r => r.ObterPorId(command.AlunoId), Times.Once);
        _alunoRepositoryMock.Verify(r => r.ObterMatriculaPorCursoEAlunoId(command.CursoId, command.AlunoId), Times.Once);
        _alunoRepositoryMock.Verify(r => r.AdicionarCertificado(It.IsAny<Certificado>()), Times.Never);
        _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<DomainNotification>(), CancellationToken.None), Times.Once);
    }

    [Fact(DisplayName = "Criar Certificado - Matricula Sem Data de Conclusao")]
    [Trait("Categoria", "GestaoAlunos - CertificadoCommandHandler")]
    public async Task AdicionarCertificado_MatriculaSemDataConclusao_NaoDeveExecutarComSucesso()
    {
        // Arrange
        var matricula = new Matricula(_alunoId, _cursoId);
        var command = new AdicionarCertificadoCommand(_alunoId, _matriculaId, _cursoId, "Curso C#");
        _alunoRepositoryMock.Setup(r => r.ObterPorId(command.AlunoId)).ReturnsAsync(_aluno);
        _alunoRepositoryMock.Setup(r => r.ObterMatriculaPorCursoEAlunoId(command.CursoId, command.AlunoId)).ReturnsAsync(matricula);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _alunoRepositoryMock.Verify(r => r.UnitOfWork.Commit(), Times.Never);
        _alunoRepositoryMock.Verify(r => r.ObterPorId(command.AlunoId), Times.Once);
        _alunoRepositoryMock.Verify(r => r.ObterMatriculaPorCursoEAlunoId(command.CursoId, command.AlunoId), Times.Once);
        _alunoRepositoryMock.Verify(r => r.AdicionarCertificado(It.IsAny<Certificado>()), Times.Never);
        _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<DomainNotification>(), CancellationToken.None), Times.Once);
    }

    [Fact(DisplayName = "Criar Certificado - PDF não encontrado")]
    [Trait("Categoria", "GestaoAlunos - CertificadoCommandHandler")]
    public async Task AdicionarCertificado_PdfNaoEncontrado_NaoDeveExecutarComSucesso()
    {
        // Arrange
        var command = new AdicionarCertificadoCommand(_alunoId, _matriculaId, _cursoId, "Curso C#");
        var matricula = new Matricula(_alunoId, _cursoId);
        matricula.Concluir();

        _alunoRepositoryMock.Setup(r => r.ObterPorId(command.AlunoId)).ReturnsAsync(_aluno);
        _alunoRepositoryMock.Setup(r => r.ObterMatriculaPorCursoEAlunoId(command.CursoId, command.AlunoId)).ReturnsAsync(matricula);
        _alunoRepositoryMock.Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);
        _certificadoPdfServiceMock.Setup(r => r.GerarPdf(It.IsAny<Certificado>())).Returns(new byte[0]);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _alunoRepositoryMock.Verify(r => r.UnitOfWork.Commit(), Times.Never);
        _alunoRepositoryMock.Verify(r => r.ObterPorId(command.AlunoId), Times.Once);
        _alunoRepositoryMock.Verify(r => r.AdicionarCertificado(It.IsAny<Certificado>()), Times.Never);
        _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<DomainNotification>(), CancellationToken.None), Times.Once);
    }

    [Fact(DisplayName = "Gerar PDF")]
    [Trait("Categoria", "GestaoAlunos - CertificadoService")]
    public void CertificadoService_GerarPdf_DeveExecutarComSucesso()
    {
        // Arrange
        var certificado = new Certificado("fulano", "curso teste", _matriculaId, _alunoId, DateTime.Now);

        // Act
        var result = _mocker.CreateInstance<CertificadoService>().GerarPdf(certificado);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.True(result.Length > 100, "O PDF gerado é muito pequeno e pode estar inválido.");

        var filePath = Path.Combine("C:\\Temp", "certificado_teste.pdf");
        File.WriteAllBytes(filePath, result);

        Assert.True(File.Exists(filePath));
    }
}