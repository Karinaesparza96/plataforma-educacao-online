using Moq;
using Moq.AutoMock;
using PlataformaEducacao.Core.DomainObjects.Enums;
using PlataformaEducacao.GestaoAlunos.Aplication.Queries;
using PlataformaEducacao.GestaoAlunos.Domain;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Tests;

public class AlunoQueriesTests
{
    private readonly AutoMocker _mocker;
    private readonly AlunoQueries _query;
    private readonly Guid _cursoId;
    private readonly Guid _alunoId;

    public AlunoQueriesTests()
    {
        _mocker = new AutoMocker();
        _query = _mocker.CreateInstance<AlunoQueries>();
        _cursoId = Guid.NewGuid();
        _alunoId = Guid.NewGuid();
    }

    [Fact(DisplayName = "Obter Matricula")]
    [Trait("Categoria", "GestaoAlunos - AlunoQueries")]
    public async Task ObterMatricula_MatriculaEncontrada_DeveRetornarComSucesso()
    {
        // Arrange
        var statusIniciada = new StatusMatricula
        {
            Codigo = (int)EStatusMatricula.Iniciada,
        };
        var matricula = new Matricula(_alunoId, _cursoId, statusIniciada);

        _mocker.GetMock<IAlunoRepository>().Setup(q => q.ObterMatriculaPorCursoEAlunoId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(matricula);

        // Act
        var result = await _query.ObterMatricula(_cursoId, _alunoId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_alunoId, result.AlunoId);
        Assert.Equal(_cursoId, result.CursoId);
    }

    [Fact(DisplayName = "Obter Matriculas Pendente Pagamento")]
    [Trait("Categoria", "GestaoAlunos - AlunoQueries")]
    public async Task ObterMatriculasPendentePagamento_MatriculasEncontradas_DeveRetornarComSucesso()
    {
        // Arrange
        var statusAguardandoPag = new StatusMatricula
        {
            Codigo = (int)EStatusMatricula.AguardandoPagamento,
        };
        var statusIniciada = new StatusMatricula
        {
            Codigo = (int)EStatusMatricula.Iniciada,
        };
        var matriculas = new List<Matricula>()
        {
            new(_alunoId, _cursoId, statusIniciada)
        };
        matriculas[0].AguardandoPagamento(statusAguardandoPag);

        _mocker.GetMock<IAlunoRepository>().Setup(q => q.ObterMatriculasPendentePagamento(It.IsAny<Guid>()))
            .ReturnsAsync(matriculas);

        // Act
        var result = await _query.ObterMatriculasPendentePagamento(_alunoId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Count());
        Assert.Collection(result, (item) =>
        {
           Assert.Equal(item.AlunoId, _alunoId);
           Assert.Equal(item.CursoId, _cursoId);
           Assert.Equal((int)EStatusMatricula.AguardandoPagamento, item.Status);
        });
    }
}