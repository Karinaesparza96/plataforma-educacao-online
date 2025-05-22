using PlataformaEducacao.GestaoConteudos.Aplication.Commands;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Tests;

public class AdicionarCursoCommandTests
{
    [Fact(DisplayName = "Adicionar Curso Command Válido")]
    [Trait("Categoria", "GestaoConteudos - CursoCommand")]
    public void EhValido_CommandValido_DeveEstarValido()
    {
        // Arrange
        var command = new AdicionarCursoCommand("Curso C# completo", "conteudo programatico", Guid.NewGuid(), 100);

        // Act
        var result = command.EhValido();

        // Assert
        Assert.True(result);

    }

    [Fact(DisplayName = "Adicionar Curso Command Inválido")]
    [Trait("Categoria", "GestaoConteudos - CursoCommand")]
    public void EhValido_CommandInvalido_DeveEstarInvalido()
    {
        // Arrange
        var command = new AdicionarCursoCommand("", "", Guid.Empty, 0);

        // Act
        var result = command.EhValido();

        // Assert
        Assert.False(result);
        Assert.Equal(4, command.ValidationResult.Errors.Count);
        Assert.Contains(AdicionarCursoCommandValidation.NomeErro, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(AdicionarCursoCommandValidation.ConteudoProgramaticoErro, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(AdicionarCursoCommandValidation.UsuarioCriacaoIdErro, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(AdicionarCursoCommandValidation.PrecoErro, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
    }
}