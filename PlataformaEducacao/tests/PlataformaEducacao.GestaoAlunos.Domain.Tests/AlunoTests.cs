namespace PlataformaEducacao.GestaoAlunos.Domain.Tests;

public class AlunoTests
{
    [Fact(DisplayName = "Aluno - Adicionar Matricula")]
    [Trait("Categoria", "GestaoAlunos - AdicionarMatricula")]
    public void AdicionarMatricula_NovaMatricula_StatusAguardandoPagamento()
    {
        // Arrange
        var aluno = new Aluno();
        var cursoId = Guid.NewGuid();
        var matricula = new Matricula(aluno.Id, cursoId);

        // Act
        aluno.AdicionarMatricula(matricula);

        // Assert
        Assert.Equal(1, aluno.Matriculas.Count);
        Assert.Equal(EStatusMatricula.AguardandoPagamento, matricula.Status);
    }
}