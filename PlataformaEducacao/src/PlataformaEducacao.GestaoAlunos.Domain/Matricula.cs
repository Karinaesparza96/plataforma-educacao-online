using PlataformaEducacao.Core.DomainObjects;

namespace PlataformaEducacao.GestaoAlunos.Domain;

public class Matricula : Entity
{
    public Guid AlunoId { get; private set; }
    public Guid CursoId { get; private set; }
    public DateTime DataMatricula { get; private set; }
    public EStatusMatricula Status { get; private set; }

    public Matricula(Guid alunoId, Guid cursoId)
    {
        AlunoId = alunoId;
        CursoId = cursoId;
    }
    public void AtivarMatricula()
    {
        Status = EStatusMatricula.Ativa;
        DataMatricula = DateTime.Now;
    }
    public void AguardarPagamento()
    {
        Status = EStatusMatricula.AguardandoPagamento;
    }
    
}