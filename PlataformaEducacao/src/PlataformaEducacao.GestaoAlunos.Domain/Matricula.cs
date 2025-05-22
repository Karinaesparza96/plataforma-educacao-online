using PlataformaEducacao.Core.DomainObjects;
using PlataformaEducacao.Core.DomainObjects.Enums;

namespace PlataformaEducacao.GestaoAlunos.Domain;

public class Matricula : Entity
{
    public Guid AlunoId { get; private set; }
    public Guid CursoId { get; private set; }
    public DateTime DataMatricula { get; private set; }
    public DateTime DataConclusao { get; private set; }
    public EStatusMatricula Status { get; private set; }

    // Ef relationship
    public Aluno Aluno { get; set; }

    public Matricula(Guid alunoId, Guid cursoId)
    {
        AlunoId = alunoId;
        CursoId = cursoId;
        Iniciar();
        Validar();
    }

    public void Iniciar()
    {
        Status = EStatusMatricula.Iniciada;
    }
    public void Ativar()
    {
        Status = EStatusMatricula.Ativa;
        DataMatricula = DateTime.Now;
    }
    public void AguardarPagamento()
    {
        Status = EStatusMatricula.AguardandoPagamento;
    }
    public void Concluir()
    {
        Status = EStatusMatricula.Concluida;
        DataConclusao = DateTime.Now;
    }

    private void Validar()
    {
        if (AlunoId == Guid.Empty)
            throw new DomainException("O campo AlunoId é obrigatório.");
        if (CursoId == Guid.Empty)
            throw new DomainException("O campo CursoId é obrigatório.");
    }

}