using PlataformaEducacao.Core.DomainObjects;
using PlataformaEducacao.Core.DomainObjects.Enums;

namespace PlataformaEducacao.GestaoConteudos.Domain;

public class ProgressoAula : Entity
{
    public Guid AlunoId { get; private set; }
    public Guid AulaId { get; private set; }
    public EProgressoAulaStatus Status { get; private set; }

    public ProgressoAula(Guid alunoId, Guid aulaId)
    {
        AlunoId = alunoId;
        AulaId = aulaId;
        Validar();
    }

    public void EmAndamento()
    {
        Status = EProgressoAulaStatus.EmAndamento;
    }
    
    public void ConcluirAula()
    {
        Status = EProgressoAulaStatus.Concluida;
    }

    private void Validar()
    {
        if (string.IsNullOrEmpty(AlunoId.ToString()))
            throw new DomainException("O campo AlunoId é obrigatório.");
        if (AulaId == Guid.Empty)
            throw new DomainException("O campo AulaId é obrigatório.");
    }
}