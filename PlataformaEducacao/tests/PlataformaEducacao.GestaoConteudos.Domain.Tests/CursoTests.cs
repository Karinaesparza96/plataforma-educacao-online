using PlataformaEducacao.Core.DomainObjects;

namespace PlataformaEducacao.GestaoConteudos.Domain.Tests;

public class CursoTests
{
    [Fact(DisplayName = "Novo Curso dados Inválidos")]
    [Trait("Categoria", "GestaoConteudos - Curso")]
    public void Validar_NovoCurso_DeveLancarExceptiono()
    {
        // Arrange && Act && Assert
        Assert.Throws<DomainException>(() => new Curso("", "", Guid.Empty, 0));
    }
    [Fact(DisplayName = "Associar Aula ao Curso")]
    [Trait("Categoria", "GestaoConteudos - Curso")]
    public void AdicionarAula_NovaAula_DeveAssociarCurso()
    {
        // Arrange
        var curso = new Curso("Curso C# completo", "conteudo programatico", Guid.NewGuid(), 100);
        var aula = new Aula("Aula 1", "Conteudo da aula 1");

        // Act
        curso.AdicionarAula(aula);

        // Assert
        Assert.Equal(1, curso.Aulas.Count(a => a.CursoId == curso.Id));
    }

    [Fact(DisplayName = "Associar Aula Existente ao Curso")]
    [Trait("Categoria", "GestaoConteudos - Curso")]
    public void AdicionarAula_AulaExistente_DeveLancarException()
    {
        // Arrange
        var curso = new Curso("Curso C# completo", "conteudo programatico", Guid.NewGuid(), 100);
        var aula = new Aula("Aula 1", "Conteudo da aula 1");

        // Act
        curso.AdicionarAula(aula);

        // Assert
        Assert.Throws<DomainException>(() => curso.AdicionarAula(aula));
        Assert.Equal(1, curso.Aulas.Count(a => a.CursoId == curso.Id));
    }
}