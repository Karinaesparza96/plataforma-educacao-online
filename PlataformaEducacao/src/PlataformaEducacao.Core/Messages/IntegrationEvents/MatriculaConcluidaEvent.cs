namespace PlataformaEducacao.Core.Messages.IntegrationEvents;

public class MatriculaConcluidaEvent : Event
{   
    public Guid AlunoId { get; set; }
    public Guid MatriculaId { get; set; }
    public Guid CursoId { get; set; }
    public string NomeCurso { get; set; }

    public MatriculaConcluidaEvent(Guid alunoId, Guid matriculaId, Guid cursoId, string nomeCurso)
    {
        AlunoId = alunoId;
        MatriculaId = matriculaId;
        CursoId = cursoId;
        NomeCurso = nomeCurso;
        AggregateId = alunoId;
    }
}