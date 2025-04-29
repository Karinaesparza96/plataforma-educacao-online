using Moq;
using Moq.AutoMock;
using NetDevPack.SimpleMediator;
using PlataformaEducacao.Core.DomainObjects.DTOs;
using PlataformaEducacao.Core.Messages.Queries;
using PlataformaEducacao.GestaoAlunos.Aplication.Commands;
using PlataformaEducacao.GestaoAlunos.Aplication.Handlers;
using PlataformaEducacao.GestaoAlunos.Domain;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Tests;

public class ProgressoAulaCommandHandlerTests 
{
    private readonly AutoMocker _mocker;
    private readonly ProgressoAulaCommandHandler _handler;
    private readonly Aluno _aluno;
    private readonly CursoDto _cursoDto;

    public ProgressoAulaCommandHandlerTests()
    {
        _mocker = new AutoMocker();
        _handler = _mocker.CreateInstance<ProgressoAulaCommandHandler>();
        _aluno = new Aluno();
        _cursoDto = new CursoDto
        {
            Id = Guid.NewGuid(),
            Nome = "Curso Teste"
        };
    }
    [Fact(DisplayName = "Registrar Progresso Aula - Aluno Matriculado")]
    [Trait("Categoria", "GestaoAlunos - ProgressoAulaCommandHandler")]
    public async Task AdicionarProgressoAula_AulaPertenceACursoAlunoMatriculado_DeveRegistrarProgresso()
    {
        var command = new IniciarAulaCommand(Guid.NewGuid(), Guid.NewGuid());

        _mocker.GetMock<IAlunoRepository>().Setup(r => r.ObterPorId(command.AlunoId)).ReturnsAsync(_aluno);
        _mocker.GetMock<IMediator>().Setup(m => m.Send(It.IsAny<ObterCursoPorAulaIdQuery>(), CancellationToken.None))
            .ReturnsAsync(_cursoDto);
        _mocker.GetMock<IAlunoRepository>().Setup(r => r.ObterMatriculaCursoPorAlunoId(command.AlunoId, _cursoDto.Id))
            .ReturnsAsync(new Matricula(command.AlunoId, _cursoDto.Id));
        _mocker.GetMock<IAlunoRepository>().Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _mocker.GetMock<IAlunoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
    }

    [Fact(DisplayName = "Registrar Progresso Aula - Aluno Não Matriculado")]
    [Trait("Categoria", "GestaoAlunos - ProgressoAulaCommandHandler")]
    public async Task AdicionarProgressoAula_AulaPertenceACursoAlunoNaoMatriculado_NaoDeveRegistrarProgresso()
    {
        var command = new IniciarAulaCommand(Guid.NewGuid(), Guid.NewGuid());

        _mocker.GetMock<IAlunoRepository>().Setup(r => r.ObterPorId(command.AlunoId)).ReturnsAsync(_aluno);
        _mocker.GetMock<IMediator>().Setup(m => m.Send(It.IsAny<ObterCursoPorAulaIdQuery>(), CancellationToken.None))
            .ReturnsAsync(_cursoDto);
        _mocker.GetMock<IAlunoRepository>().Setup(r => r.ObterMatriculaCursoPorAlunoId(command.AlunoId, _cursoDto.Id))
            .ReturnsAsync((Matricula?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _mocker.GetMock<IAlunoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Never);
    }
}