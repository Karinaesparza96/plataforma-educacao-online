using Moq;
using Moq.AutoMock;
using PlataformaEducacao.GestaoConteudos.Aplication.Queries;
using PlataformaEducacao.GestaoConteudos.Domain;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Tests;

public class CursoQueriesTests
{
    private readonly AutoMocker _mocker;
    private readonly CursoQueries _query;
    private readonly Guid _aulaId;
    private readonly Curso _curso;

    public CursoQueriesTests()
    {
        _mocker = new AutoMocker();
        _query = _mocker.CreateInstance<CursoQueries>();
        _aulaId = Guid.NewGuid();
        _curso = new Curso("Curso Teste", "Descricao Teste", Guid.NewGuid(), 10);
    }

    [Fact(DisplayName = "Obter Curso Por Id")]
    [Trait("Categoria", "GestaoConteudos - CursoQueries")]
    public async Task ObterCursoPorId_CursoEncontrado_DeveRetornarComSucesso()
    {
        // Arrange
        var aula = new Aula("Aula 1", "Conteúdo 1");
        _curso.AdicionarAula(aula);
        _mocker.GetMock<ICursoRepository>().Setup(q => q.ObterPorId(_curso.Id))
            .ReturnsAsync(_curso);

        // Act
        var result = await _query.ObterPorId(_curso.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_curso.Id, result.Id);
        Assert.Equal(_curso.Nome, result.Nome);
        Assert.Equal(_curso.ConteudoProgramatico, result.ConteudoProgramatico);
        Assert.Equal(_curso.Preco, result.Preco);
        Assert.Single(result.Aulas);
        Assert.Equal(aula.Id, result.Aulas.First().Id);
    }
}