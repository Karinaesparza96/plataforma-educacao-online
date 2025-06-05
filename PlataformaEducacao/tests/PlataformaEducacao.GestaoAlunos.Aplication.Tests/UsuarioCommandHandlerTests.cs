using Moq;
using Moq.AutoMock;
using PlataformaEducacao.Core.DomainObjects;
using PlataformaEducacao.GestaoAlunos.Aplication.Commands;
using PlataformaEducacao.GestaoAlunos.Aplication.Handlers;
using PlataformaEducacao.GestaoAlunos.Domain;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Tests;

public class UsuarioCommandHandlerTests
{
    [Fact(DisplayName = "Novo Aluno - AdicionarAlunoCommand")]
    [Trait("Categoria", "GestaoAlunos - UsuarioCommandHandler")]
    public async Task Adicionar_NovoAluno_DeveSalvarComSucesso()
    {
        // Arrange
        var usuarioId = Guid.NewGuid().ToString();
        var command = new AdicionarAlunoCommand(usuarioId, "fulano");
        var mocker = new AutoMocker();
        mocker.GetMock<IAlunoRepository>()
            .Setup(x => x.UnitOfWork.Commit())
            .ReturnsAsync(true);
        var handler = mocker.CreateInstance<UsuarioCommandHandler>();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        mocker.GetMock<IAlunoRepository>()
            .Verify(x => x.Adicionar(It.IsAny<Aluno>()), Times.Once);
        mocker.GetMock<IAlunoRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Once);
    }


    [Fact(DisplayName = "Novo Usuario Admin - AdicionarAdminCommand")]
    [Trait("Categoria", "GestaoAlunos - UsuarioCommandHandler")]
    public async Task Adicionar_NovoAdmin_DeveSalvarComSucesso()
    {
        // Arrange
        var usuarioId = Guid.NewGuid().ToString();
        var command = new AdicionarAdminCommand(usuarioId);
        var mocker = new AutoMocker();
        mocker.GetMock<IUsuarioRepository>()
            .Setup(x => x.UnitOfWork.Commit())
            .ReturnsAsync(true);
        var handler = mocker.CreateInstance<UsuarioCommandHandler>();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        mocker.GetMock<IUsuarioRepository>()
            .Verify(x => x.Adicionar(It.IsAny<Usuario>()), Times.Once);
        mocker.GetMock<IUsuarioRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Once);
    }

    [Fact(DisplayName = "Novo Aluno - AdicionarAlunoCommand Invalido")]
    [Trait("Categoria", "GestaoAlunos - UsuarioCommandHandler")]
    public async Task Adicionar_CommandInvalido_NaoDeveSalvarComSucesso()
    {
        // Arrange
        var command = new AdicionarAlunoCommand(string.Empty, string.Empty);
        var mocker = new AutoMocker();

        var handler = mocker.CreateInstance<UsuarioCommandHandler>();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        mocker.GetMock<IAlunoRepository>()
            .Verify(x => x.Adicionar(It.IsAny<Aluno>()), Times.Never);
        mocker.GetMock<IAlunoRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Never);
    }


    [Fact(DisplayName = "Novo Usuario Admin - AdicionarAdminCommand Invalido")]
    [Trait("Categoria", "GestaoAlunos - UsuarioCommandHandler")]
    public async Task Adicionar_AdminCommandInvalido_NaoDeveSalvarComSucesso()
    {
        // Arrange
        var command = new AdicionarAdminCommand(string.Empty);
        var mocker = new AutoMocker();
        var handler = mocker.CreateInstance<UsuarioCommandHandler>();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        mocker.GetMock<IUsuarioRepository>()
            .Verify(x => x.Adicionar(It.IsAny<Usuario>()), Times.Never);
        mocker.GetMock<IUsuarioRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Never);
    }
}