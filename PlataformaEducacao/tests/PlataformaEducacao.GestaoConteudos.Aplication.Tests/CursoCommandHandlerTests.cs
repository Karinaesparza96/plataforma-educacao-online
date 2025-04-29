using Moq;
using Moq.AutoMock;
using PlataformaEducacao.GestaoConteudos.Aplication.Commands;
using PlataformaEducacao.GestaoConteudos.Aplication.Handlers;
using PlataformaEducacao.GestaoConteudos.Domain;

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
        var command = new AdicionarCursoCommand("Curso C# completo", "conteudo programatico", Guid.NewGuid());

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
        var command = new AdicionarCursoCommand("", "", Guid.NewGuid());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        
        _mocker.GetMock<ICursoRepository>().Verify(r => r.Adicionar(It.IsAny<Curso>()), Times.Never);
        _mocker.GetMock<ICursoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Never);
    }
}