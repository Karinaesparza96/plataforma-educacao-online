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
        aula.AdicionarProgresso(progressoAula);

        // Act
        aula.ConcluirAula(progressoAula);

        // Assert
        Assert.True(progressoAula.Status == EProgressoAulaStatus.Concluida);
    }

    [Fact(DisplayName = "Concluir Aula Sem Sucesso")]
    [Trait("Categoria", "GestaoConteudos - Aula")]
    public void ConcluirAula_ProgressoNaoEncontrado_DeveLancarException()
    {
        // Arrange
        var aula = new Aula("Aula 1", "Conteudo da aula 1");
        var progressoAula = new ProgressoAula(Guid.NewGuid(), aula.Id);

        // Act && Assert
        Assert.Throws<DomainException>(() => aula.ConcluirAula(progressoAula));
    }

    [Fact(DisplayName = "Adicionar ProgressoAula")]
    [Trait("Categoria", "GestaoConteudos - Aula")]
    public void AdicionarProgresso_NovoProgresso_DeveAssociarAulaComSucesso()
    {
        // Arrange
        var aula = new Aula("Aula 1", "Conteudo da aula 1");
        var progressoAula = new ProgressoAula(Guid.NewGuid(), aula.Id);

        // Act
        aula.AdicionarProgresso(progressoAula);

        // Assert
        Assert.True(progressoAula.Status == EProgressoAulaStatus.EmAndamento);
        Assert.Equal(1, aula.ProgressoAulas.Count(a => a.AulaId == aula.Id && a.AlunoId == progressoAula.AlunoId));
    }

    [Fact(DisplayName = "Adicionar Em Duplicidade ProgressoAula")]
    [Trait("Categoria", "GestaoConteudos - Aula")]
    public void AdicionarProgresso_ProgressoExistente_DeveLancarException()
    {
        // Arrange
        var aula = new Aula("Aula 1", "Conteudo da aula 1");
        var progressoAula = new ProgressoAula(Guid.NewGuid(), aula.Id);

        // Act
        aula.AdicionarProgresso(progressoAula);

        // Assert
        Assert.Throws<DomainException>(() => aula.AdicionarProgresso(progressoAula));
        Assert.Equal(1, aula.ProgressoAulas.Count(a => a.AulaId == aula.Id && a.AlunoId == progressoAula.AlunoId));
    }

    [Fact(DisplayName = "Filtras ProgressoAula por AlunoId")]
    [Trait("Categoria", "GestaoConteudos - Aula")]
    public void FiltrarProgressoAulaPorAlunoId_DeveRetornarFiltrado()
    {
        // Arrange
        var alunoId1 = Guid.NewGuid();
        var alunoId2 = Guid.NewGuid();
        var aula = new Aula("Aula 1", "Conteudo da aula");
        var progressoAula1 = new ProgressoAula(alunoId1, aula.Id);
        var progressoAula2 = new ProgressoAula(alunoId2, aula.Id);
        aula.AdicionarProgresso(progressoAula1);
        aula.AdicionarProgresso(progressoAula2);

        // Act
        aula.FiltrarProgressoAulaPorAluno(alunoId1);

        // Assert
        Assert.Equal(1, aula.ProgressoAulas.Count());
    }
}