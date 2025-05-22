using PlataformaEducacao.Core.DomainObjects;

namespace PlataformaEducacao.GestaoAlunos.Domain.Tests;

public class MatriculaTests
{
    [Fact(DisplayName = "Nova Matricula - Dados Inválidos")]
    [Trait("Categoria ", "GestaoAlunos - Matricula")]
    public void Validar_NovaMatricula_DeveLancarException()
    {
        // Arrange && Act && Assert
        Assert.Throws<DomainException>(() => new Matricula(Guid.Empty, Guid.Empty));

    }
}