﻿using PlataformaEducacao.GestaoConteudos.Aplication.Commands;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Tests;

public class ValidarPagamentoCursoCommandTests
{
    [Fact(DisplayName = "Realizar Pagamento Curso Command Válido")]
    [Trait("Categoria", "GestaoConteudos - RealizarPagamentoCursoCommand")]
    public void EhValido_CommandValido_DeveEstarValido()
    {
        // Arrange
        var command = new ValidarPagamentoCursoCommand(
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
        var command = new ValidarPagamentoCursoCommand(
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
        Assert.Contains(ValidarPagamentoCursoCommandValidation.CursoIdErro, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(ValidarPagamentoCursoCommandValidation.AlunoIdErro, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(ValidarPagamentoCursoCommandValidation.NomeCartaoErro, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(ValidarPagamentoCursoCommandValidation.NumeroCartaoErro, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(ValidarPagamentoCursoCommandValidation.ExpiracaoCartaoErro, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(ValidarPagamentoCursoCommandValidation.CvvCartaoErro, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
    }
}