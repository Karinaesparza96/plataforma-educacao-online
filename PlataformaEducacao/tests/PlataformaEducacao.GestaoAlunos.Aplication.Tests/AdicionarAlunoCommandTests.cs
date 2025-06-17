﻿using PlataformaEducacao.GestaoAlunos.Aplication.Commands;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Tests;

public class AdicionarAlunoCommandTests
{
    [Fact(DisplayName = "Novo Aluno Command Valido")]
    [Trait("Categoria", "GestaoAlunos - AdicionarAlunoCommand")]
    public void EhValido_CommandValido_DeveEstarValido()
    {
        // Arrange
        var usuarioId = Guid.NewGuid().ToString();
        var command = new AdicionarAlunoCommand(usuarioId, "fulano");

        // Act
        var ehValido = command.EhValido();

        // Assert
        Assert.True(ehValido);
    }

    [Fact(DisplayName = "Novo Aluno Command Invalido")]
    [Trait("Categoria", "GestaoAlunos - AdicionarAlunoCommand")]
    public void EhValido_CommandInvalido_DeveConterErros()
    {
        // Arrange
        var command = new AdicionarAlunoCommand(string.Empty, "");

        // Act
        var ehValido = command.EhValido();

        // Assert
        Assert.False(ehValido);
        Assert.Contains(AdicionarAlunoCommandValidation.IdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(AdicionarAlunoCommandValidation.NomeErro, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Equal(2, command.ValidationResult.Errors.Count);
    }
}