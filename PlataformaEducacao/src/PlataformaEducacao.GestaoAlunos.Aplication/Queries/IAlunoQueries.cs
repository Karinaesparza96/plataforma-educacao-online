using PlataformaEducacao.GestaoAlunos.Aplication.Queries.ViewModels;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Queries;

public interface IAlunoQueries
{
    Task<MatriculaViewModel?> ObterMatriculaPorAlunoECursoId(Guid cursoId, Guid alunoId);
}