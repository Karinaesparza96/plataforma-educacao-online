using Moq;
using Moq.AutoMock;
using PlataformaEducacao.GestaoAlunos.Aplication.Commands;
using PlataformaEducacao.GestaoAlunos.Aplication.Handlers;
using PlataformaEducacao.GestaoAlunos.Domain;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Tests;

public class MatriculaCommandHandlerTests
{
    private readonly AutoMocker _mocker;
    private readonly MatriculaCommandHandler _handler;
    private readonly Mock<IAlunoRepository> _alunoRepositoryMock;
    private readonly Aluno _aluno;
    private readonly Guid _cursoId;
    private readonly Guid _alunoId;

    public MatriculaCommandHandlerTests()
    {
        _mocker = new AutoMocker();
        _handler = _mocker.CreateInstance<MatriculaCommandHandler>();
        _alunoRepositoryMock = _mocker.GetMock<IAlunoRepository>();
        _aluno = new Aluno();
        _cursoId = Guid.NewGuid();
        _alunoId = Guid.NewGuid(); 
    }
    [Fact(DisplayName = "Criar Matricula Com Sucesso - Curso Disponivel")]
    [Trait("Categoria", "GestaoAlunos - MatriculaCommandHandler")]
    public async Task AdicionarMatricula_CursoDisponivel_DeveExecutarComSucesso()
    {
        // Arrange
        var command = new CriarMatriculaCommand(_aluno.Id, _cursoId);

        _alunoRepositoryMock.Setup(r => r.ObterPorId(_aluno.Id)).ReturnsAsync(_aluno);
        _alunoRepositoryMock.Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        Assert.Equal(EStatusMatricula.AguardandoPagamento, _aluno.ObterMatricula(_cursoId)?.Status);
        _alunoRepositoryMock.Verify(r => r.UnitOfWork.Commit(), Times.Once);
        _alunoRepositoryMock.Verify(r => r.ObterPorId(_aluno.Id), Times.Once);
    }

    [Fact(DisplayName = "Criar Matricula Com Erro - Aluno Não Encontrado")]
    [Trait("Categoria", "GestaoAlunos - MatriculaCommandHandler")]
    public async Task AdicionarMatricula_AlunoNãoEncontrado_NãoDeveExecutarComSucesso()
    {
        // Arrange
        var command = new CriarMatriculaCommand(_alunoId, _cursoId);

        _mocker.GetMock<IAlunoRepository>().Setup(r => r.ObterPorId(_alunoId)).ReturnsAsync((Aluno?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _alunoRepositoryMock.Verify(r => r.UnitOfWork.Commit(), Times.Never);
        _alunoRepositoryMock.Verify(r => r.ObterPorId(_alunoId), Times.Once);
    }
}