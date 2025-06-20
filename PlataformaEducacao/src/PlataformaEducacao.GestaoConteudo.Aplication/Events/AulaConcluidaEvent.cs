using PlataformaEducacao.Core.Messages;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Events;

public class AulaConcluidaEvent : Event
{
    public Guid AulaId { get; set; }
    public Guid AlunoId { get; set; }
    public Guid CursoId { get; set; }

    public AulaConcluidaEvent(Guid aulaId, Guid alunoId, Guid cursoId)
    {
        AulaId = aulaId;
        AlunoId = alunoId;
        CursoId = cursoId;
    }
}