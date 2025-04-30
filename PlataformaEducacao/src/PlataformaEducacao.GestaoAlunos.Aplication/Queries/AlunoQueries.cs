using PlataformaEducacao.GestaoAlunos.Aplication.Queries.ViewModels;
using PlataformaEducacao.GestaoAlunos.Domain;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Queries;

public class AlunoQueries(IAlunoRepository alunoRepository) : IAlunoQueries
{
    public async Task<MatriculaViewModel?> ObterMatriculaPorAlunoECursoId(Guid cursoId, Guid alunoId)
    {
        var matricula = await alunoRepository.ObterMatriculaCursoPorAlunoId(cursoId, alunoId);

        if (matricula == null)
            return null;

        return new MatriculaViewModel
        {
            AlunoId = matricula.AlunoId,
            CursoId = matricula.CursoId,
            Status = matricula.Status,
            DataMatricula = matricula.DataMatricula
        };
    }
}