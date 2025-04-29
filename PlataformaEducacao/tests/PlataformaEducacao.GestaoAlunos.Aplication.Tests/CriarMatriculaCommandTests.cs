using PlataformaEducacao.GestaoAlunos.Aplication.Commands;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Tests;

public class CriarMatriculaCommandTests
{
    [Fact(DisplayName = "Criar Matricula Command Valido")]
    [Trait("Categoria", "GestaoAlunos - CriarMatriculaCommand")]
    public void EhValido_CommandValido_DeveEstarValido()
    {
        // Arrange
        var command = new CriarMatriculaCommand(Guid.NewGuid(), Guid.NewGuid());

        // Act
        var result = command.EhValido();

        // Assert
        Assert.True(result);
    }

    [Fact(DisplayName = "Criar Matricula Command Invalido")]
    [Trait("Categoria", "GestaoAlunos - CriarMatriculaCommand")]
    public void EhValido_CommandInvalido_DeveEstarInvalido()
    {
        // Arrange
        var command = new CriarMatriculaCommand(Guid.Empty, Guid.Empty);

        // Act
        var result = command.EhValido();

        // Assert
        Assert.False(result);
        Assert.Equal(2, command.ValidationResult.Errors.Count);
        Assert.Contains(CriarMatriculaCommandValidation.AlunoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(CriarMatriculaCommandValidation.CursoIdErro, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
    }
}