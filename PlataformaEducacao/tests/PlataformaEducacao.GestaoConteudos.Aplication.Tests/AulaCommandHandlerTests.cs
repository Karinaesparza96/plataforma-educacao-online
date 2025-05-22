using MediatR;
using Moq;
using Moq.AutoMock;
using PlataformaEducacao.Core.DomainObjects.DTOs;
using PlataformaEducacao.Core.DomainObjects.Enums;
using PlataformaEducacao.Core.Messages;
using PlataformaEducacao.Core.Messages.IntegrationQueries;
using PlataformaEducacao.GestaoConteudos.Aplication.Commands;
using PlataformaEducacao.GestaoConteudos.Aplication.Handlers;
using PlataformaEducacao.GestaoConteudos.Domain;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Tests;

public class AulaCommandHandlerTests
{
    private readonly AutoMocker _mocker;
    private readonly AulaCommandHandler _handler;
    private readonly Guid _cursoId = Guid.NewGuid();
    private readonly Guid _aulaId = Guid.NewGuid();
    private readonly Guid _alunoId = Guid.NewGuid();

    public AulaCommandHandlerTests()
    {
        _mocker = new AutoMocker();
        _handler = _mocker.CreateInstance<AulaCommandHandler>();
    }

    [Fact(DisplayName = "Adicionar Aula Command Válido")]
    [Trait("Categoria", "GestaoConteudos - AulaCommandHandler")]
    public async Task Adicionar_CommandValido_DeveExecutarComSucesso()
    {
        // Arrange
        var command = new AdicionarAulaCommand("Aula 1", "Conteudo da aula", Guid.NewGuid(), "nome material", "ZIP");
        
        _mocker.GetMock<ICursoRepository>().Setup(x => x.ObterPorId(command.CursoId)).ReturnsAsync(new Curso("Curso", "teste", Guid.NewGuid(), 100));
        _mocker.GetMock<ICursoRepository>().Setup(x => x.UnitOfWork.Commit()).Returns(Task.FromResult(true));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _mocker.GetMock<ICursoRepository>().Verify(r => r.Adicionar(It.IsAny<Aula>()), Times.Once);
        _mocker.GetMock<ICursoRepository>().Verify(x => x.ObterPorId(command.CursoId), Times.Once);
        _mocker.GetMock<ICursoRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Once);
    }

    [Fact(DisplayName = "Adicionar Aula Command Inválido")]
    [Trait("Categoria", "GestaoConteudos - AulaCommandHandler")]
    public async Task Adicionar_CommandInvalido_NaoDeveExecutarComSucesso()
    {
        // Arrange
        var command = new AdicionarAulaCommand("", "", Guid.Empty, "nome material", "ZIP");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _mocker.GetMock<ICursoRepository>().Verify(r => r.Adicionar(It.IsAny<Aula>()), Times.Never);
        _mocker.GetMock<ICursoRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Never);
    }

    [Fact(DisplayName = "Adicionar Aula Command - Curso não encontrado")]
    [Trait("Categoria", "GestaoConteudos - AulaCommandHandler")]
    public async Task Adicionar_CursoNaoEncontrado_NaoDeveExecutarComSucesso()
    {
        // Arrange
        var command = new AdicionarAulaCommand("Aula 1", "Conteudo da aula", Guid.NewGuid(), "nome material", "ZIP");
        _mocker.GetMock<ICursoRepository>().Setup(r => r.ObterPorId(command.CursoId)).ReturnsAsync((Curso?)null);
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _mocker.GetMock<ICursoRepository>().Verify(r => r.ObterPorId(command.CursoId), Times.Once);
        _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<DomainNotification>(), CancellationToken.None), Times.Once);
    }

    [Fact(DisplayName = "Realizar Aula Command Invalido")]
    [Trait("Categoria", "GestaoConteudos - AulaCommandHandler")]
    public async Task AdicionarProgresso_CommandInvalido_NaoDeveExecutarComSucesso()
    {
        // Arrange
        var command = new RealizarAulaCommand(Guid.Empty, Guid.Empty, Guid.Empty);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _mocker.GetMock<ICursoRepository>().Verify(r => r.Adicionar(It.IsAny<Aula>()), Times.Never);
        _mocker.GetMock<ICursoRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Never);
    }

    [Fact(DisplayName = "Realizar Aula Command - Com Sucesso")]
    [Trait("Categoria", "GestaoConteudos - AulaCommandHandler")]
    public async Task AdicionarProgresso_CommandValido_DeveExecutarComSucesso()
    {
        // Arrange
        var command = new RealizarAulaCommand(_aulaId, _alunoId, _cursoId);
        _mocker.GetMock<ICursoRepository>().Setup(x => x.ObterAulaPorId(command.AulaId)).ReturnsAsync(new Aula("Aula 1", "Conteudo da aula"));
        _mocker.GetMock<IAulaRepository>().Setup(x => x.UnitOfWork.Commit()).Returns(Task.FromResult(true));
        _mocker.GetMock<IMediator>()
            .Setup(x => x.Send(It.IsAny<ObterMatriculaCursoAlunoQuery>(), CancellationToken.None)).ReturnsAsync(
                new MatriculaDto
                {
                    Id = Guid.NewGuid(),
                    Status = EStatusMatricula.Ativa
                });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _mocker.GetMock<IAulaRepository>().Verify(r => r.AdicionarProgressoAula(It.IsAny<ProgressoAula>()), Times.Once);
        _mocker.GetMock<IAulaRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Once);
    }
    [Fact(DisplayName = "Realizar Aula Command - Aula não encontrada")]
    [Trait("Categoria", "GestaoConteudos - AulaCommandHandler")]
    public async Task AdicionarProgresso_AulaNaoEncontrada_NaoDeveExecutarComSucesso()
    {
        // Arrange
        var command = new RealizarAulaCommand(_aulaId, _alunoId, _cursoId);
        _mocker.GetMock<ICursoRepository>().Setup(x => x.ObterAulaPorId(command.AulaId)).ReturnsAsync((Aula)null);
        _mocker.GetMock<IMediator>().Setup(a => a.Send(It.IsAny<ObterMatriculaCursoAlunoQuery>(), CancellationToken.None))
            .ReturnsAsync(new MatriculaDto { Status = EStatusMatricula.Ativa });
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        Assert.False(result);
        _mocker.GetMock<ICursoRepository>().Verify(x => x.ObterAulaPorId(command.AulaId), Times.Once);
       _mocker.GetMock<IAulaRepository>().Verify(r => r.AdicionarProgressoAula(It.IsAny<ProgressoAula>()), Times.Never);
        _mocker.GetMock<IAulaRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Never);
        _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<DomainNotification>(), CancellationToken.None), Times.Once);
    }

    [Fact(DisplayName = "Realizar Aula Command - Matricula Invalida")]
    [Trait("Categoria", "GestaoConteudos - AulaCommandHandler")]
    public async Task AdicionarProgresso_MatriculaInvalida_NaoDeveExecutarComSucesso()
    {
        // Arrange
        var command = new RealizarAulaCommand(_aulaId, _alunoId, _cursoId);
        _mocker.GetMock<ICursoRepository>().Setup(x => x.ObterAulaPorId(command.AulaId)).ReturnsAsync((Aula)null);
        _mocker.GetMock<IMediator>().Setup(x => x.Send(It.IsAny<ObterMatriculaCursoAlunoQuery>(), CancellationToken.None))
            .ReturnsAsync(new MatriculaDto
            {
                Status = EStatusMatricula.AguardandoPagamento
            });
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        Assert.False(result);
        _mocker.GetMock<ICursoRepository>().Verify(x => x.ObterAulaPorId(command.AulaId), Times.Never);
        _mocker.GetMock<IAulaRepository>().Verify(r => r.AdicionarProgressoAula(It.IsAny<ProgressoAula>()), Times.Never);
        _mocker.GetMock<IAulaRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Never);
        _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<DomainNotification>(), CancellationToken.None), Times.Once);
    }

    [Fact(DisplayName = "Concluir Aula Command - Com Sucesso")]
    [Trait("Categoria", "GestaoConteudos - AulaCommandHandler")]
    public async Task ConcluirAula_CommandValido_DeveExecutarComSucesso()
    {
        // Arrange
        var command = new ConcluirAulaCommand(_aulaId, _alunoId, _cursoId);
        var aula = new Aula("Aula 1", "Conteudo da aula");
        var progressoAula = new ProgressoAula(command.AlunoId, aula.Id);
        aula.AdicionarProgresso(progressoAula);
        
        _mocker.GetMock<ICursoRepository>().Setup(x => x.ObterAulaPorId(command.AulaId)).ReturnsAsync(aula);
        _mocker.GetMock<IAulaRepository>().Setup(x => x.ObterProgressoAula(aula.Id, command.AlunoId)).ReturnsAsync(progressoAula);
        _mocker.GetMock<IAulaRepository>().Setup(x => x.UnitOfWork.Commit()).Returns(Task.FromResult(true));
        _mocker.GetMock<IMediator>().Setup(a => a.Send(It.IsAny<ObterMatriculaCursoAlunoQuery>(), CancellationToken.None))
            .ReturnsAsync(new MatriculaDto { Status = EStatusMatricula.Ativa });
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _mocker.GetMock<IAulaRepository>().Verify(r => r.AdicionarProgressoAula(It.IsAny<ProgressoAula>()), Times.Once);
        _mocker.GetMock<IAulaRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Once);
    }

    [Fact(DisplayName = "Concluir Aula Command - Command Invalido")]
    [Trait("Categoria", "GestaoConteudos - AulaCommandHandler")]
    public async Task ConcluirAula_CommandInvalido_NaoDeveExecutarComSucesso()
    {
        // Arrange
        var command = new ConcluirAulaCommand(Guid.Empty, Guid.Empty, Guid.Empty);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        Assert.False(result);
        _mocker.GetMock<ICursoRepository>().Verify(x => x.ObterAulaPorId(command.AulaId), Times.Never);
        _mocker.GetMock<IAulaRepository>().Verify(r => r.AdicionarProgressoAula(It.IsAny<ProgressoAula>()), Times.Never);
        _mocker.GetMock<IAulaRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Never);
    }

    [Fact(DisplayName = "Concluir Aula Command - Aula não encontrada")]
    [Trait("Categoria", "GestaoConteudos - AulaCommandHandler")]
    public async Task ConcluirAula_AulaNaoEncontrada_NaoDeveExecutarComSucesso()
    {
        // Arrange
        var command = new ConcluirAulaCommand(_aulaId, _alunoId, _cursoId);
        _mocker.GetMock<ICursoRepository>().Setup(x => x.ObterAulaPorId(command.AulaId)).ReturnsAsync((Aula)null);
        _mocker.GetMock<IMediator>().Setup(a => a.Send(It.IsAny<ObterMatriculaCursoAlunoQuery>(), CancellationToken.None))
            .ReturnsAsync(new MatriculaDto {Status = EStatusMatricula.Ativa});

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        Assert.False(result);
        _mocker.GetMock<ICursoRepository>().Verify(x => x.ObterAulaPorId(command.AulaId), Times.Once);
        _mocker.GetMock<IAulaRepository>().Verify(r => r.AdicionarProgressoAula(It.IsAny<ProgressoAula>()), Times.Never);
        _mocker.GetMock<IAulaRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Never);
        _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<DomainNotification>(), CancellationToken.None), Times.Once);
    }

    [Fact(DisplayName = "Concluir Aula Command - Matricula Invalida")]
    [Trait("Categoria", "GestaoConteudos - AulaCommandHandler")]
    public async Task ConcluirAula_MatriculaInvalida_NaoDeveExecutarComSucesso()
    {
        // Arrange
        var command = new ConcluirAulaCommand(_aulaId, _alunoId, _cursoId);
        _mocker.GetMock<ICursoRepository>().Setup(x => x.ObterAulaPorId(command.AulaId)).ReturnsAsync((Aula)null);
        _mocker.GetMock<IMediator>().Setup(a => a.Send(It.IsAny<ObterMatriculaCursoAlunoQuery>(), CancellationToken.None))
            .ReturnsAsync(new MatriculaDto { Status = EStatusMatricula.AguardandoPagamento });
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        Assert.False(result);
        _mocker.GetMock<ICursoRepository>().Verify(x => x.ObterAulaPorId(command.AulaId), Times.Never);
        _mocker.GetMock<IAulaRepository>().Verify(r => r.AdicionarProgressoAula(It.IsAny<ProgressoAula>()), Times.Never);
        _mocker.GetMock<IAulaRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Never);
        _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<DomainNotification>(), CancellationToken.None), Times.Once);
    }

    [Fact(DisplayName = "Concluir Aula Command - Progresso não encontrado")]
    [Trait("Categoria", "GestaoConteudos - AulaCommandHandler")]
    public async Task ConcluirAula_ProgressoNaoEncontrado_NaoDeveExecutarComSucesso()
    {
        // Arrange
        var command = new ConcluirAulaCommand(_aulaId, _alunoId, _cursoId);
        var aula = new Aula("Aula 1", "Conteudo da aula");
        _mocker.GetMock<ICursoRepository>().Setup(x => x.ObterAulaPorId(command.AulaId)).ReturnsAsync(aula);
        _mocker.GetMock<IAulaRepository>().Setup(x => x.ObterProgressoAula(aula.Id, command.AlunoId)).ReturnsAsync((ProgressoAula)null);
        _mocker.GetMock<IMediator>().Setup(a => a.Send(It.IsAny<ObterMatriculaCursoAlunoQuery>(), CancellationToken.None))
            .ReturnsAsync(new MatriculaDto { Status = EStatusMatricula.Ativa });
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        Assert.False(result);
        _mocker.GetMock<ICursoRepository>().Verify(r => r.ObterAulaPorId(command.AulaId), Times.Once);
        _mocker.GetMock<IAulaRepository>().Verify(r => r.AdicionarProgressoAula(It.IsAny<ProgressoAula>()), Times.Never);
        _mocker.GetMock<IAulaRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Never);
        _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<DomainNotification>(), CancellationToken.None), Times.Once);
    }
}