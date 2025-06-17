using PlataformaEducacao.GestaoAlunos.Aplication.Commands;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Tests;

public class AdicionarAdminCommandTests
{
    [Fact(DisplayName = "Novo Admin Command Valido")]
    [Trait("Categoria", "GestaoAlunos - AdicionarAdminCommand")]
    public void EhValido_CommandAdminValido_DeveEstarValido()
    {
        // Arrange
        var usuarioId = Guid.NewGuid().ToString();
        var command = new AdicionarAdminCommand(usuarioId);

        // Act
        var ehValido = command.EhValido();

        // Assert
        Assert.True(ehValido);
    }

    [Fact(DisplayName = "Novo Admin Command Invalido")]
    [Trait("Categoria", "GestaoAlunos - AdicionarAdminCommand")]
    public void EhValido_CommandAdminInvalido_DeveConterErros()
    {
        // Arrange
        var usuarioId = string.Empty;
        var command = new AdicionarAdminCommand(usuarioId);

        // Act
        var ehValido = command.EhValido();

        // Assert
        Assert.False(ehValido);
        Assert.Contains(AdicionarAlunoCommandValidation.IdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Equal(1, command.ValidationResult.Errors.Count);
    }
}