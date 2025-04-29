using PlataformaEducacao.GestaoConteudos.Aplication.Commands;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Tests;

public class IniciarAulaCommandTests
{
    [Fact(DisplayName = "Iniciar Aula Command Valido")]
    [Trait("Categoria", "GestaoConteudos - IniciarAulaCommand")]
    public void EhValido_CommandValido_DeveEstarValido()
    {
        // Arrange
        var aulaId = Guid.NewGuid();
        var alunoId = Guid.NewGuid();
        var command = new IniciarAulaCommand(aulaId, alunoId);

        // Act
        var result = command.EhValido();

        // Assert
        Assert.True(result);
    }

    [Fact(DisplayName = "Iniciar Aula Command Invalido")]
    [Trait("Categoria", "GestaoConteudos - IniciarAulaCommand")]
    public void EhValido_CommandInvalido_DeveEstarInvalido()
    {
        // Arrange
        var aulaId = Guid.Empty;
        var alunoId = Guid.Empty;
        var command = new IniciarAulaCommand(aulaId, alunoId);

        // Act
        var result = command.EhValido();

        // Assert
        Assert.False(result);
        Assert.Equal(2, command.ValidationResult.Errors.Count);
        Assert.Contains(IniciarAulaCommandValidation.AulaIdErro, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(IniciarAulaCommandValidation.AlunoIdErro, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
    }
}