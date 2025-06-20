using PlataformaEducacao.Core.DomainObjects;
using PlataformaEducacao.Core.DomainObjects.Enums;

namespace PlataformaEducacao.GestaoConteudos.Domain;

public class ProgressoAula : Entity
{
    public Guid AlunoId { get; private set; }
    public Guid AulaId { get; private set; }
    public EProgressoAulaStatus Status { get; private set; }

    protected ProgressoAula() { }

    public ProgressoAula(Guid alunoId, Guid aulaId)
    {
        AlunoId = alunoId;
        AulaId = aulaId;
        Status = EProgressoAulaStatus.NaoIniciada;
        Validar();
    }
    public void EmAndamento() => Status = EProgressoAulaStatus.EmAndamento;
    public void ConcluirAula() => Status = EProgressoAulaStatus.Concluida;

    public void Validar()
    {
        if (AlunoId == Guid.Empty)
            throw new DomainException("O ID do aluno não pode ser vazio.");
        if (AulaId == Guid.Empty)
            throw new DomainException("O ID da aula não pode ser vazio.");
    }
}