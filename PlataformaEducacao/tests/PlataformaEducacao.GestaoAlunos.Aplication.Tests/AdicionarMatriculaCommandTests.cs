using MediatR;
using Moq;
using Moq.AutoMock;
using PlataformaEducacao.Core.DomainObjects.DTOs;
using PlataformaEducacao.Core.DomainObjects.Enums;
using PlataformaEducacao.Core.Messages.IntegrationQueries;
using PlataformaEducacao.Core.Messages.Notifications;
using PlataformaEducacao.GestaoAlunos.Aplication.Commands;
using PlataformaEducacao.GestaoAlunos.Aplication.Handlers;
using PlataformaEducacao.GestaoAlunos.Domain;
using Matricula = PlataformaEducacao.GestaoAlunos.Domain.Matricula;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Tests;

public class AdicionarMatriculaCommandTests
{
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
        var mocker = new AutoMocker();
        var aluno = new Aluno(Guid.NewGuid(), "teste");
        var handler = mocker.CreateInstance<MatriculaCommandHandler>();
        mocker.GetMock<IAlunoRepository>().Setup(x => x.ObterPorId(command.AlunoId)).ReturnsAsync(aluno);
        mocker.GetMock<IAlunoRepository>().Setup(x => x.UnitOfWork.Commit()).Returns(Task.FromResult(true));
        mocker.GetMock<IMediator>()
            .Setup(m => m.Send(It.IsAny<ObterCursoQuery>(), CancellationToken.None)).ReturnsAsync(new CursoDto { Id = command.CursoId });

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        mocker.GetMock<IAlunoRepository>().Verify(x => x.ObterPorId(command.AlunoId), Times.Once);
        mocker.GetMock<IAlunoRepository>().Verify(x => x.AdicionarMatricula(It.IsAny<Matricula>()), Times.Once);
        mocker.GetMock<IAlunoRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Once);
        mocker.GetMock<IMediator>()
            .Verify(m => m.Send(It.IsAny<ObterCursoQuery>(), CancellationToken.None), Times.Once);
    }

    [Fact(DisplayName = "Concluir Matricula Command Valido")]
    [Trait("Categoria", "GestaoAlunos - MatriculaCommandHandler")]
    public async Task ConcluirMatricula_CommandValido_DeveSalvarComSucesso()
    {
        // Arrange
        var command = new ConcluirMatriculaCommand(Guid.NewGuid(), Guid.NewGuid());
        var mocker = new AutoMocker();
        var matricula = new Matricula(command.AlunoId, command.CursoId);
        var handler = mocker.CreateInstance<MatriculaCommandHandler>();

        mocker.GetMock<IAlunoRepository>().Setup(x => x.ObterMatriculaPorCursoEAlunoId(command.CursoId, command.AlunoId)).ReturnsAsync(matricula);
        mocker.GetMock<IAlunoRepository>().Setup(x => x.UnitOfWork.Commit()).Returns(Task.FromResult(true));
        mocker.GetMock<IMediator>()
            .Setup(m => m.Send(It.IsAny<ObterAulasCursoAlunoQuery>(), CancellationToken.None))
            .ReturnsAsync(new CursoDto
            {
                Aulas = [
                    new AulaDto
                    {
                        Status = EProgressoAulaStatus.Concluida
                    }
                ]
            });

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        mocker.GetMock<IAlunoRepository>().Verify(x => x.AtualizarMatricula(matricula), Times.Once);
        mocker.GetMock<IAlunoRepository>().Verify(x => x.ObterMatriculaPorCursoEAlunoId(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
        mocker.GetMock<IAlunoRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Once);
        mocker.GetMock<IMediator>()
            .Verify(m => m.Send(It.IsAny<ObterAulasCursoAlunoQuery>(), CancellationToken.None), Times.Once);
    }

    [Fact(DisplayName = "Concluir Matricula - Aulas não concluidas")]
    [Trait("Categoria", "GestaoAlunos - MatriculaCommandHandler")]
    public async Task ConcluirMatricula_AulasNaoConcluidas_NaoDeveSalvarComSucesso()
    {
        // Arrange
        var command = new ConcluirMatriculaCommand(Guid.NewGuid(), Guid.NewGuid());
        var mocker = new AutoMocker();
        var handler = mocker.CreateInstance<MatriculaCommandHandler>();

        mocker.GetMock<IMediator>()
            .Setup(m => m.Send(It.IsAny<ObterAulasCursoAlunoQuery>(), CancellationToken.None))
            .ReturnsAsync(new CursoDto
            {
                Aulas = [
                    new AulaDto
                    {
                        Status = EProgressoAulaStatus.Concluida
                    },
                    new AulaDto
                    {
                        Status = EProgressoAulaStatus.EmAndamento
                    }
                ]
            });

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        mocker.GetMock<IAlunoRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Never);
        mocker.GetMock<IMediator>()
            .Verify(m => m.Send(It.IsAny<ObterAulasCursoAlunoQuery>(), CancellationToken.None), Times.Once);
        mocker.GetMock<IMediator>()
            .Verify(m => m.Publish(It.IsAny<DomainNotification>(), CancellationToken.None), Times.Once);
    }
}