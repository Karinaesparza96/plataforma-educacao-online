namespace PlataformaEducacao.Core.Messages.IntegrationEvents;

public class MatriculaConcluidaEvent : Event
{   
    public Guid AlunoId { get; set; }
    public Guid MatriculaId { get; set; }
    public Guid CursoId { get; set; }

    public MatriculaConcluidaEvent(Guid alunoId, Guid matriculaId, Guid cursoId)
    {
        AlunoId = alunoId;
        MatriculaId = matriculaId;
        CursoId = cursoId;
        AggregateId = alunoId;
    }
}