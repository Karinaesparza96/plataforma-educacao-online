using PlataformaEducacao.GestaoConteudos.Aplication.Commands;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Tests;

public class RealizarPagamentoCursoCommandTests
{
    [Fact(DisplayName = "Realizar Pagamento Curso Command Válido")]
    [Trait("Categoria", "GestaoConteudos - RealizarPagamentoCursoCommand")]
    public void EhValido_CommandValido_DeveEstarValido()
    {
        // Arrange
        var command = new RealizarPagamentoCursoCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Nome do Cartão",
            "5502093788528294",
            "12/25",
            "455"
        );

        // Act
        var result = command.EhValido();

        // Assert
        Assert.True(result);
    }
    [Fact(DisplayName = "Realizar Pagamento Curso Command Inválido")]
    [Trait("Categoria", "GestaoConteudos - RealizarPagamentoCursoCommand")]
    public void EhValido_CommandInvalido_DeveEstarInvalido()
    {
        // Arrange
        var command = new RealizarPagamentoCursoCommand(
            Guid.Empty,
            Guid.Empty,
            "",
            "",
            "",
            ""
        );
        // Act
        var result = command.EhValido();
        // Assert
        Assert.False(result);
        Assert.Equal(6, command.ValidationResult.Errors.Count);
        Assert.Contains(RealizarPagamentoCursoCommandValidation.CursoId, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(RealizarPagamentoCursoCommandValidation.AlunoId, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(RealizarPagamentoCursoCommandValidation.NomeCartao, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(RealizarPagamentoCursoCommandValidation.NumeroCartao, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(RealizarPagamentoCursoCommandValidation.ExpiracaoCartao, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(RealizarPagamentoCursoCommandValidation.CvvCartao, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
    }
}