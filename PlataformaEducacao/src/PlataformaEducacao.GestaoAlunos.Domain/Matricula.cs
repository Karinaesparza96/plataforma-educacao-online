using PlataformaEducacao.Core.DomainObjects;
using PlataformaEducacao.Core.DomainObjects.Enums;

namespace PlataformaEducacao.GestaoAlunos.Domain;

public class Matricula : Entity
{
    public Guid AlunoId { get; private set; }
    public Guid CursoId { get; private set; }
    public DateTime? DataMatricula { get; private set; }
    public DateTime? DataConclusao { get; private set; }
    public Guid StatusId { get; private set; }

    // Ef relationship
    public Aluno Aluno { get; set; }
    public StatusMatricula Status { get; private set; }

    // Ef Constructor
    protected Matricula() { }
    public Matricula(Guid alunoId, Guid cursoId, StatusMatricula status)
    {
        AlunoId = alunoId;
        CursoId = cursoId;
        Validar();
        Iniciar(status);
    }

    public void Iniciar(StatusMatricula status)
    {
        if (status?.Codigo != (int)EStatusMatricula.Iniciada)
            throw new DomainException("A matrícula deve estar com o status 'Iniciada'.");

        AssociarStatus(status);
    }

    public void Ativar(StatusMatricula status)
    {
        if (status?.Codigo != (int)EStatusMatricula.Ativa)
            throw new DomainException("A matrícula deve estar com o status 'Ativa'.");

        AssociarStatus(status);
        DataMatricula = DateTime.Now;
    }
    public void Concluir(StatusMatricula status)
    {   
        if (status?.Codigo != (int)EStatusMatricula.Concluida)
            throw new DomainException("A matrícula deve estar com o status 'Concluída'.");

        AssociarStatus(status);
        DataConclusao = DateTime.Now;
    }
    public void AguardandoPagamento(StatusMatricula status)
    {
        if (status?.Codigo != (int)EStatusMatricula.AguardandoPagamento)
            throw new DomainException("A matrícula deve estar com o status 'Aguardando Pagamento'.");

        AssociarStatus(status);
    }

    private void AssociarStatus(StatusMatricula status)
    {
        if (status == null)
            throw new DomainException("O status da matrícula não pode ser nulo.");
        Status = status;
    }

    private void Validar()
    {
        if (AlunoId == Guid.Empty)
            throw new DomainException("O campo AlunoId é obrigatório.");
        if (CursoId == Guid.Empty)
            throw new DomainException("O campo CursoId é obrigatório.");
    }

}