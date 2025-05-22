using PlataformaEducacao.GestaoAlunos.Aplication.Commands;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Tests;

public class AtivarMatriculaCommandTests
{
    [Fact(DisplayName = "Ativar Matricula Command Valido")]
    [Trait("Categoria", "GestaoAlunos - AtivarMatriculaCommand")]
    public void EhValido_CommandValido_DeveEstarValido()
    {
        // Arrange
        var command = new AtivarMatriculaCommand(Guid.NewGuid(), Guid.NewGuid());

        // Act
        var result = command.EhValido();

        // Assert
        Assert.True(result);
    }

    [Fact(DisplayName = "Ativar Matricula Command Invalido")]
    [Trait("Categoria", "GestaoAlunos - AtivarMatriculaCommand")]
    public void EhValido_CommandInvalido_DeveEstarInvalido()
    {
        // Arrange
        var command = new AtivarMatriculaCommand(Guid.Empty, Guid.Empty);

        // Act
        var result = command.EhValido();

        // Assert
        Assert.False(result);
        Assert.Equal(2, command.ValidationResult.Errors.Count);
        Assert.Contains(AtivarMatriculaCommandValidation.AlunoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(AtivarMatriculaCommandValidation.CursoIdErro, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
    }
}