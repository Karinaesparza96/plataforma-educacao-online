namespace PlataformaEducacao.Core.Messages.IntegrationEvents;

public class CursoPagamentoRealizadoEvent(Guid cursoId, Guid alunoId) : Event
{
    public Guid CursoId { get; set; } = cursoId;
    public Guid AlunoId { get; set; } = alunoId;
}