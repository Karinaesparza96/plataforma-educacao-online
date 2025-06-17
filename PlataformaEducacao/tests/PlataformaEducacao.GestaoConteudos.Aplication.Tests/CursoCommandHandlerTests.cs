using MediatR;
using Moq;
using Moq.AutoMock;
using PlataformaEducacao.Core.Messages.Notifications;
using PlataformaEducacao.GestaoConteudos.Aplication.Commands;
using PlataformaEducacao.GestaoConteudos.Aplication.Handlers;
using PlataformaEducacao.GestaoConteudos.Domain;
using Curso = PlataformaEducacao.GestaoConteudos.Domain.Curso;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Tests;

public class CursoCommandHandlerTests
{
    private readonly AutoMocker _mocker;
    private readonly CursoCommandHandler _handler;

    public CursoCommandHandlerTests()
    {
        _mocker = new AutoMocker();
        _handler = _mocker.CreateInstance<CursoCommandHandler>();
    }
    [Fact(DisplayName = "Adicionar Novo Curso CommandValido")]
    [Trait("Categoria", "GestaoConteudos - CursoCommandHandler")]
    public async Task Adicionar_NovoCurso_DeveExecutarComSucesso()
    {
        // Arrange
        var command = new AdicionarCursoCommand("Curso C# completo", "conteudo programatico", Guid.NewGuid(), 100);

        _mocker.GetMock<ICursoRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _mocker.GetMock<ICursoRepository>().Verify(r => r.Adicionar(It.IsAny<Curso>()), Times.Once);
        _mocker.GetMock<ICursoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
    }

    [Fact(DisplayName = "Adicionar Novo Curso CommandInvalido")]
    [Trait("Categoria", "GestaoConteudos - CursoCommandHandler")]
    public async Task Adicionar_NovoCurso_NaoDeveExecutarComSucesso()
    {
        // Arrange
        var command = new AdicionarCursoCommand("", "", Guid.Empty,0);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        
        _mocker.GetMock<ICursoRepository>().Verify(r => r.Adicionar(It.IsAny<Curso>()), Times.Never);
        _mocker.GetMock<ICursoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Never);
    }

    [Fact(DisplayName = "Atualizar Curso - Curso Existente")]
    [Trait("Categoria", "GestaoConteudos - CursoCommandHandler")]
    public async Task Atualizar_CursoExistente_DeveExecutarComSucesso()
    {
        // Arrange
        var curso = new Curso("Curso Angular", "teste", Guid.NewGuid(), 100);

        var command = new AtualizarCursoCommand(curso.Id,"Curso C# completo", "conteudo programatico", 100);
        _mocker.GetMock<ICursoRepository>().Setup(r => r.ObterPorId(command.CursoId)).ReturnsAsync(curso);
        _mocker.GetMock<ICursoRepository>()
            .Setup(r => r.UnitOfWork.Commit())
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _mocker.GetMock<ICursoRepository>().Verify(r => r.Atualizar(It.IsAny<Curso>()), Times.Once);
        _mocker.GetMock<ICursoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
    }
    [Fact(DisplayName = "Atualizar Curso - Curso Nao Encontrado")]
    [Trait("Categoria", "GestaoConteudos - CursoCommandHandler")]
    public async Task Atualizar_CursoNaoEncontrado_NaoDeveExecutarComSucesso()
    {
        // Arrange
        var command = new AtualizarCursoCommand(Guid.NewGuid(), "Curso C# completo", "conteudo programatico", 100);
        _mocker.GetMock<ICursoRepository>().Setup(r => r.ObterPorId(command.CursoId)).ReturnsAsync((Curso?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _mocker.GetMock<ICursoRepository>().Verify(r => r.Atualizar(It.IsAny<Curso>()), Times.Never);
        _mocker.GetMock<ICursoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Never);
        _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<DomainNotification>(), CancellationToken.None), Times.Once);
    }

    [Fact(DisplayName = "Deletar Curso com Sucesso")]
    [Trait("Categoria", "GestaoConteudos - CursoCommandHandler")]
    public async Task Deletar_CursoExistente_DeveExecutarComSucesso()
    {
        // Arrange
        var curso = new Curso("Curso Angular", "teste", Guid.NewGuid(), 100);
        var command = new DeletarCursoCommand(curso.Id);
        _mocker.GetMock<ICursoRepository>().Setup(r => r.ObterCursoComAulas(command.CursoId)).ReturnsAsync(curso);
        _mocker.GetMock<ICursoRepository>()
            .Setup(r => r.UnitOfWork.Commit())
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _mocker.GetMock<ICursoRepository>().Verify(r => r.Remover(It.IsAny<Curso>()), Times.Once);
        _mocker.GetMock<ICursoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
    }

    [Fact(DisplayName = "Deletar Curso com Erro")]
    [Trait("Categoria", "GestaoConteudos - CursoCommandHandler")]
    public async Task Deletar_CommandInvalido_NaoDeveExecutarComSucesso()
    {
        // Arrange
        var command = new DeletarCursoCommand(Guid.Empty);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        Assert.Contains(DeletarCursoCommandValidation.CursoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Equal(1, command.ValidationResult.Errors.Count);
    }

    [Fact(DisplayName = "Deletar Curso com Erro")]
    [Trait("Categoria", "GestaoConteudos - CursoCommandHandler")]
    public async Task Deletar_CursoNaoEncontrado_NaoDeveExecutarComSucesso()
    {
        // Arrange
        var command = new DeletarCursoCommand(Guid.NewGuid());
        _mocker.GetMock<ICursoRepository>().Setup(r => r.ObterCursoComAulas(command.CursoId)).ReturnsAsync((Curso?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _mocker.GetMock<ICursoRepository>().Verify(r => r.Remover(It.IsAny<Curso>()), Times.Never);
        _mocker.GetMock<ICursoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Never);
        _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<DomainNotification>(), CancellationToken.None), Times.Once);
    }

    [Fact(DisplayName = "Deletar Curso com Erro")]
    [Trait("Categoria", "GestaoConteudos - CursoCommandHandler")]
    public async Task Deletar_CursoComAulas_NaoDeveExecutarComSucesso()
    {
        // Arrange
        var curso = new Curso("Curso Angular", "teste", Guid.NewGuid(), 100);
        curso.AdicionarAula(new Aula("Aula 1", "Conteudo da aula 1"));

        var command = new DeletarCursoCommand(curso.Id);
        _mocker.GetMock<ICursoRepository>().Setup(r => r.ObterCursoComAulas(command.CursoId)).ReturnsAsync(curso);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _mocker.GetMock<ICursoRepository>().Verify(r => r.Remover(It.IsAny<Curso>()), Times.Never);
        _mocker.GetMock<ICursoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Never);
        _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<DomainNotification>(), CancellationToken.None), Times.Once);
    }
}