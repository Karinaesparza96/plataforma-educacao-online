using PlataformaEducacao.Core.DomainObjects;

namespace PlataformaEducacao.GestaoConteudos.Domain.Tests;

public class AulaTests
{
    [Fact(DisplayName = "Adicionar Material a Aula")]
    [Trait("Categoria", "GestaoConteudos - Aula")]
    public void AdicionarMaterial_MaterialValido_DeveAssociarAula()
    {
        // Arrange
        var aula = new Aula("Aula 1", "Conteudo da aula 1");
        var material = new Material("Material 1", "ZIP");

        // Act
        aula.AdicionarMaterial(material);

        // Assert
        Assert.Equal(1, aula.Materiais.Count(a => a.AulaId == aula.Id));
    }

    [Fact(DisplayName = "Adicionar Material Existente a Aula")]
    [Trait("Categoria", "GestaoConteudos - Aula")]
    public void AdicionarMaterial_MaterialExistente_DeveLancarException()
    {
        // Arrange
        var aula = new Aula("Aula 1", "Conteudo da aula 1");
        var material = new Material("Material 1", "ZIP");

        // Act
        aula.AdicionarMaterial(material);

        // Assert
        Assert.Throws<DomainException>(() => aula.AdicionarMaterial(material));
        Assert.Equal(1, aula.Materiais.Count(a => a.AulaId == aula.Id));
    }
}