using PlataformaEducacao.GestaoAlunos.Aplication.Commands;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Tests;

public class IniciarAulaCommandTests
{
    [Fact(DisplayName = "Iniciar Aula Command Valido")]
    [Trait("Categoria", "GestaoAlunos - IniciarAulaCommand")]
    public void EhValido_CommandValido_DeveEstarValido()
    {
        // Arrange
        var command = new IniciarAulaCommand(Guid.NewGuid(), Guid.NewGuid());

        // Act
        var result = command.EhValido();

        // Assert
        Assert.True(result);
    }

    [Fact(DisplayName = "Iniciar Aula Command Invalido")]
    [Trait("Categoria", "GestaoAlunos - IniciarAulaCommand")]
    public void EhValido_CommandInvalido_DeveEstarInvalido()
    {
        // Arrange
        var command = new IniciarAulaCommand(Guid.Empty, Guid.Empty);
        // Act
        var result = command.EhValido();
        // Assert
        Assert.False(result);
        Assert.Equal(2, command.ValidationResult.Errors.Count);
        Assert.Contains(IniciarAulaCommandValidation.AulaIdErro, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(IniciarAulaCommandValidation.AlunoIdErro, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
    }
}