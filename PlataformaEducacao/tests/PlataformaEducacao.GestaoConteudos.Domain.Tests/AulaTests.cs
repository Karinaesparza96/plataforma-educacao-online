using PlataformaEducacao.Core.DomainObjects;
using PlataformaEducacao.Core.DomainObjects.Enums;

namespace PlataformaEducacao.GestaoConteudos.Domain.Tests;

public class AulaTests
{
    [Fact(DisplayName = "Nova Aula Dados Inválidos")]
    [Trait("Categoria", "GestaoConteudos - Aula")]
    public void Validar_NovaAula_DeveLancarException()
    {
        // Arrange && Act && Assert
        Assert.Throws<DomainException>(() => new Aula("", ""));

    }
    [Fact(DisplayName = "Novo Material Dados Inválidos")]
    [Trait("Categoria", "GestaoConteudos - Aula")]
    public void Validar_NovoMaterial_DeveLancarException()
    {
        // Arrange && Act && Assert
        Assert.Throws<DomainException>(() => new Material("", ""));
    }

    [Fact(DisplayName = "Novo ProgressoAula Dados Inválidos")]
    [Trait("Categoria", "GestaoConteudos - Aula")]
    public void Validar_NovoProgressoAula_DeveLancarException()
    {
        // Arrange && Act && Assert
        Assert.Throws<DomainException>(() => new ProgressoAula(Guid.Empty, Guid.Empty));
    }
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

    [Fact(DisplayName = "Concluir Aula Com Sucesso")]
    [Trait("Categoria", "GestaoConteudos - Aula")]
    public void ConcluirAula_ProgressoEncontrado_DeveExecutarComSucesso()
    {
        // Arrange
        var aula = new Aula("Aula 1", "Conteudo da aula 1");
        var progressoAula = new ProgressoAula(aula.Id, Guid.NewGuid());

        // Act
        progressoAula.ConcluirAula();

        // Assert
        Assert.True(progressoAula.Status == EProgressoAulaStatus.Concluida);
    }

    [Fact(DisplayName = "Adicionar ProgressoAula")]
    [Trait("Categoria", "GestaoConteudos - Aula")]
    public void AdicionarProgresso_NovoProgresso_DeveAssociarAulaComSucesso()
    {
        // Arrange
        var aula = new Aula("Aula 1", "Conteudo da aula 1");
        var progressoAula = new ProgressoAula(Guid.NewGuid(), aula.Id);

        // Act
        progressoAula.EmAndamento();

        // Assert
        Assert.True(progressoAula.Status == EProgressoAulaStatus.EmAndamento);
    }
}