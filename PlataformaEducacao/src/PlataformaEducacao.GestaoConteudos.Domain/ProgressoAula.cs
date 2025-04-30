using PlataformaEducacao.Core.DomainObjects;

namespace PlataformaEducacao.GestaoConteudos.Domain;

public class ProgressoAula(Guid alunoId, Guid aulaId) : Entity
{
    public Guid AlunoId { get; private set; } = alunoId;
    public Guid AulaId { get; private set; } = aulaId;
    public EProgressoAulaStatus Status { get; private set; } = EProgressoAulaStatus.EmAndamento;

    public void ConcluirAula()
    {
        Status = EProgressoAulaStatus.Concluida;
    }
}

public enum EProgressoAulaStatus
{
    EmAndamento = 1,
    Concluida = 2,
}