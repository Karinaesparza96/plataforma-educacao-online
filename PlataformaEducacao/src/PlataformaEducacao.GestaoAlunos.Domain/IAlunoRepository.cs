using PlataformaEducacao.Core.Data;

namespace PlataformaEducacao.GestaoAlunos.Domain;

public interface IAlunoRepository : IRepository<Aluno>
{
   Task<Aluno?> ObterPorId(Guid id);

   Task<Matricula?> ObterMatriculaCursoPorAlunoId(Guid alunoId, Guid cursoId);
}