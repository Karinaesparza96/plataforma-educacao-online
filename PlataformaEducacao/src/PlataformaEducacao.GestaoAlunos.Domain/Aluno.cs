using PlataformaEducacao.Core.DomainObjects;

namespace PlataformaEducacao.GestaoAlunos.Domain;

public class Aluno : Entity, IAggregateRoot
{
    private readonly List<Matricula> _matriculas = [];
    public IReadOnlyCollection<Matricula> Matriculas => _matriculas;
    
    public void AdicionarMatricula(Matricula matricula)
    {
        if (MatriculaExistente(matricula))
            throw new DomainException("Matrícula já existente.");

        matricula.AguardarPagamento();

        _matriculas.Add(matricula);
    }

    public Matricula? ObterMatricula(Guid cursoId)
    {
        return _matriculas.FirstOrDefault(m => m.CursoId == cursoId && m.AlunoId == Id);
    }
    
    private bool MatriculaExistente(Matricula matricula)
    {
        return _matriculas.Any(m => m.Id == matricula.Id);
    }
}