using MediatR;
using Moq;
using Moq.AutoMock;
using PlataformaEducacao.Core.Messages.IntegrationEvents;
using PlataformaEducacao.GestaoAlunos.Aplication.Commands;
using PlataformaEducacao.GestaoAlunos.Aplication.Handlers;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Tests;

public class MatriculaEventHandlerTests
{
    private readonly AutoMocker _mocker;
    private readonly MatriculaEventHandler _handler;
    private readonly Guid _cursoId;
    private readonly Guid _alunoId;
    private readonly Guid _matriculaId;

    public MatriculaEventHandlerTests()
    {
        _mocker = new AutoMocker();
        _handler = _mocker.CreateInstance<MatriculaEventHandler>();
        _cursoId = Guid.NewGuid();
        _alunoId = Guid.NewGuid();
        _matriculaId = Guid.NewGuid();
    }
    [Fact(DisplayName = "Curso Pagamento Realizado Event")]
    [Trait("Categoria", "GestaoAlunos - MatriculaEventHandler")]
    public async Task Handle_CursoPagamentoRealizadoEvent_DeveExecutarComSucesso()
    {
        // Arrange
        var command = new CursoPagamentoRealizadoEvent(_alunoId, _cursoId);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mocker.GetMock<IMediator>().Verify(m => m.Send(It.IsAny<AtivarMatriculaCommand>(), CancellationToken.None), Times.Once);
    }
    [Fact(DisplayName = "Matricula Concluida Event")]
    [Trait("Categoria", "GestaoAlunos - MatriculaEventHandler")]
    public async Task Handle_MatriculaConcluidaEvent_DeveExecutarComSucesso()
    {
        // Arrange
        var command = new MatriculaConcluidaEvent(_alunoId, _matriculaId, _cursoId);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mocker.GetMock<IMediator>().Verify(m => m.Send(It.IsAny<AdicionarCertificadoCommand>(), CancellationToken.None), Times.Once);
    }
}