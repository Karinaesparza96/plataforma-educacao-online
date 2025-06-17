using MediatR;
using Moq;
using Moq.AutoMock;
using PlataformaEducacao.Core.Messages.Notifications;
using PlataformaEducacao.GestaoAlunos.Aplication.Commands;
using PlataformaEducacao.GestaoAlunos.Aplication.Handlers;
using PlataformaEducacao.GestaoAlunos.Domain;
using Matricula = PlataformaEducacao.GestaoAlunos.Domain.Matricula;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Tests;

public class AdicionarMatriculaCommandTests
{   
    private readonly AutoMocker _mocker;
    private readonly MatriculaCommandHandler _handler;
    public AdicionarMatriculaCommandTests()
    {
        _mocker = new AutoMocker();
        _handler = _mocker.CreateInstance<MatriculaCommandHandler>();
    }
    [Fact(DisplayName = "Criar Matricula Command Valido")]
    [Trait("Categoria", "GestaoAlunos - AdicionarMatriculaCommand")]
    public void EhValido_CommandValido_DeveEstarValido()
    {
        // Arrange
        var command = new AdicionarMatriculaCommand(Guid.NewGuid(), Guid.NewGuid());

        // Act
        var result = command.EhValido();

        // Assert
        Assert.True(result);
    }

    [Fact(DisplayName = "Criar Matricula Command Invalido")]
    [Trait("Categoria", "GestaoAlunos - AdicionarMatriculaCommand")]
    public void EhValido_CommandInvalido_DeveEstarInvalido()
    {
        // Arrange
        var command = new AdicionarMatriculaCommand(Guid.Empty, Guid.Empty);

        // Act
        var result = command.EhValido();

        // Assert
        Assert.False(result);
        Assert.Equal(2, command.ValidationResult.Errors.Count);
        Assert.Contains(AdicionarMatriculaCommandValidation.AlunoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(AdicionarMatriculaCommandValidation.CursoIdErro, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
    }

    [Fact(DisplayName = "Criar Matricula Command Valido")]
    [Trait("Categoria", "GestaoAlunos - MatriculaCommandHandler")]
    public async Task AdicionarMatricula_CommandValido_DeveSalvarComSucesso()
    {
        // Arrange
        var command = new AdicionarMatriculaCommand(Guid.NewGuid(), Guid.NewGuid());
        var aluno = new Aluno(Guid.NewGuid(), "teste");

        _mocker.GetMock<IAlunoRepository>().Setup(x => x.ObterPorId(command.AlunoId)).ReturnsAsync(aluno);
        _mocker.GetMock<IAlunoRepository>().Setup(x => x.UnitOfWork.Commit()).Returns(Task.FromResult(true));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _mocker.GetMock<IAlunoRepository>().Verify(x => x.ObterPorId(command.AlunoId), Times.Once);
        _mocker.GetMock<IAlunoRepository>().Verify(x => x.AdicionarMatricula(It.IsAny<Matricula>()), Times.Once);
        _mocker.GetMock<IAlunoRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Once);
    }

    [Fact(DisplayName = "Concluir Matricula Command - Matricula Encontrada")]
    [Trait("Categoria", "GestaoAlunos - MatriculaCommandHandler")]
    public async Task ConcluirMatricula_MatriculaEncontrada_DeveSalvarComSucesso()
    {
        // Arrange
        var command = new ConcluirMatriculaCommand(Guid.NewGuid(), Guid.NewGuid(), "Curso C#");
        var matricula = new Matricula(command.AlunoId, command.CursoId);

        _mocker.GetMock<IAlunoRepository>().Setup(x => x.ObterMatriculaPorCursoEAlunoId(command.CursoId, command.AlunoId)).ReturnsAsync(matricula);
        _mocker.GetMock<IAlunoRepository>().Setup(x => x.UnitOfWork.Commit()).Returns(Task.FromResult(true));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _mocker.GetMock<IAlunoRepository>().Verify(x => x.AtualizarMatricula(matricula), Times.Once);
        _mocker.GetMock<IAlunoRepository>().Verify(x => x.ObterMatriculaPorCursoEAlunoId(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
        _mocker.GetMock<IAlunoRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Once);
    }

    [Fact(DisplayName = "Concluir Matricula - Matricula não encontrada")]
    [Trait("Categoria", "GestaoAlunos - MatriculaCommandHandler")]
    public async Task ConcluirMatricula_MatriculaNaoEncontrada_NaoDeveSalvarComSucesso()
    {
        // Arrange
        var command = new ConcluirMatriculaCommand(Guid.NewGuid(), Guid.NewGuid(), "Curso C#");
        _mocker.GetMock<IAlunoRepository>().Setup(x => x.ObterMatriculaPorCursoEAlunoId(command.CursoId, command.AlunoId))
            .ReturnsAsync((Matricula)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _mocker.GetMock<IAlunoRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Never);
        _mocker.GetMock<IAlunoRepository>().Verify(x => x.ObterMatriculaPorCursoEAlunoId(command.CursoId, command.AlunoId), Times.Once);
        _mocker.GetMock<IMediator>()
            .Verify(m => m.Publish(It.IsAny<DomainNotification>(), CancellationToken.None), Times.Once);
    }

    [Fact(DisplayName = "Concluir Matricula Com Erro - Command")]
    [Trait("Categoria", "GestaoAlunos - MatriculaCommandHandler")]
    public async Task ConcluirMatricula_CommandInvalido_NaoDeveSalvarComSucesso()
    {
        // Arrange
        var command = new ConcluirMatriculaCommand(Guid.Empty, Guid.Empty, "");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _mocker.GetMock<IAlunoRepository>().Verify(x => x.ObterMatriculaPorCursoEAlunoId(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
        _mocker.GetMock<IAlunoRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Never);
        Assert.Contains(ConcluirMatriculaCommandValidation.AlunoId, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(ConcluirMatriculaCommandValidation.CursoId, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(ConcluirMatriculaCommandValidation.NomeCurso, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Equal(3, command.ValidationResult.Errors.Count);
    }
}