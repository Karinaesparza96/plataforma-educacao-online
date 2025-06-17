using Moq;
using Moq.AutoMock;
using PlataformaEducacao.GestaoAlunos.Aplication.Commands;
using PlataformaEducacao.GestaoAlunos.Aplication.Handlers;
using PlataformaEducacao.GestaoAlunos.Domain;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Tests;

public class UsuarioCommandHandlerTests
{   
    private readonly AutoMocker _mocker;
    private readonly UsuarioCommandHandler _handler;
    public UsuarioCommandHandlerTests()
    {
        _mocker = new AutoMocker();
        _handler = _mocker.CreateInstance<UsuarioCommandHandler>();
    }

    [Fact(DisplayName = "Novo Aluno - AdicionarAlunoCommand")]
    [Trait("Categoria", "GestaoAlunos - UsuarioCommandHandler")]
    public async Task Adicionar_NovoAluno_DeveSalvarComSucesso()
    {
        // Arrange
        var usuarioId = Guid.NewGuid().ToString();
        var command = new AdicionarAlunoCommand(usuarioId, "fulano");
        _mocker.GetMock<IAlunoRepository>()
            .Setup(x => x.UnitOfWork.Commit())
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _mocker.GetMock<IAlunoRepository>()
            .Verify(x => x.Adicionar(It.IsAny<Aluno>()), Times.Once);
        _mocker.GetMock<IAlunoRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Once);
    }


    [Fact(DisplayName = "Novo Usuario Admin - AdicionarAdminCommand")]
    [Trait("Categoria", "GestaoAlunos - UsuarioCommandHandler")]
    public async Task Adicionar_NovoAdmin_DeveSalvarComSucesso()
    {
        // Arrange
        var usuarioId = Guid.NewGuid().ToString();
        var command = new AdicionarAdminCommand(usuarioId);
        _mocker.GetMock<IUsuarioRepository>()
            .Setup(x => x.UnitOfWork.Commit())
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _mocker.GetMock<IUsuarioRepository>()
            .Verify(x => x.Adicionar(It.IsAny<Usuario>()), Times.Once);
        _mocker.GetMock<IUsuarioRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Once);
    }

    [Fact(DisplayName = "Novo Aluno - AdicionarAlunoCommand Invalido")]
    [Trait("Categoria", "GestaoAlunos - UsuarioCommandHandler")]
    public async Task Adicionar_CommandInvalido_NaoDeveSalvarComSucesso()
    {
        // Arrange
        var command = new AdicionarAlunoCommand(string.Empty, string.Empty);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _mocker.GetMock<IAlunoRepository>()
            .Verify(x => x.Adicionar(It.IsAny<Aluno>()), Times.Never);
        _mocker.GetMock<IAlunoRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Never);
    }


    [Fact(DisplayName = "Novo Usuario Admin - AdicionarAdminCommand Invalido")]
    [Trait("Categoria", "GestaoAlunos - UsuarioCommandHandler")]
    public async Task Adicionar_AdminCommandInvalido_NaoDeveSalvarComSucesso()
    {
        // Arrange
        var command = new AdicionarAdminCommand(string.Empty);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _mocker.GetMock<IUsuarioRepository>()
            .Verify(x => x.Adicionar(It.IsAny<Usuario>()), Times.Never);
        _mocker.GetMock<IUsuarioRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Never);
    }
}