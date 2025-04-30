using PlataformaEducacao.GestaoConteudos.Aplication.Commands;
using PlataformaEducacao.GestaoConteudos.Domain;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Tests;

public class RealizarAulaCommandTests
{
    [Fact(DisplayName = "Realizar Aula Command Valido")]
    [Trait("Categoria", "GestaoConteudos - RealizarAulaCommand")]
    public void EhValido_CommandValido_DeveEstarValido()
    {
        // Arrange
        var aulaId = Guid.NewGuid();
        var alunoId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();
        var command = new RealizarAulaCommand(aulaId, alunoId, cursoId);

        // Act
        var result = command.EhValido();

        // Assert
        Assert.True(result);
    }

    [Fact(DisplayName = "Realizar Aula Command Invalido")]
    [Trait("Categoria", "GestaoConteudos - RealizarAulaCommand")]
    public void EhValido_CommandInvalido_DeveEstarInvalido()
    {
        // Arrange
        var aulaId = Guid.Empty;
        var alunoId = Guid.Empty;
        var cursoId = Guid.Empty;
        var command = new RealizarAulaCommand(aulaId, alunoId, cursoId);

        // Act
        var result = command.EhValido();

        // Assert
        Assert.False(result);
        Assert.Equal(3, command.ValidationResult.Errors.Count);
        Assert.Contains(RealizarAulaCommandValidation.AulaIdErro, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(RealizarAulaCommandValidation.AlunoIdErro, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(RealizarAulaCommandValidation.CursoIdErro, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
    }
}