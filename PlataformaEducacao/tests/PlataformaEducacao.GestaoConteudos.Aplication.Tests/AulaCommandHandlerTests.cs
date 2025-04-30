using Moq;
using Moq.AutoMock;
using PlataformaEducacao.GestaoConteudos.Aplication.Commands;
using PlataformaEducacao.GestaoConteudos.Aplication.Handlers;
using PlataformaEducacao.GestaoConteudos.Domain;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Tests;

public class AulaCommandHandlerTests
{
    private readonly AutoMocker _mocker;
    private readonly AulaCommandHandler _handler;

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
        
        _mocker.GetMock<ICursoRepository>().Setup(x => x.ObterPorId(command.CursoId)).ReturnsAsync(new Curso("Curso", "teste", Guid.NewGuid()));
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
}