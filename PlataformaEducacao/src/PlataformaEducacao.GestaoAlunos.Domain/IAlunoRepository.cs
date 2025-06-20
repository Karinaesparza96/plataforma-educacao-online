using PlataformaEducacao.Core.Data;
using PlataformaEducacao.Core.DomainObjects;

namespace PlataformaEducacao.GestaoAlunos.Domain;

public interface IAlunoRepository : IRepository<Aluno>, IAggregateRoot
{
   Task<Aluno?> ObterPorId(Guid id);
   Task<Matricula?> ObterMatriculaPorCursoEAlunoId(Guid cursoId, Guid alunoId);
   Task<IEnumerable<Matricula>> ObterMatriculasPendentePagamento(Guid alunoId);
   Task<Certificado?> ObterCertificadoPorId(Guid certificadoId, Guid alunoId);

   void AdicionarMatricula(Matricula matricula);
   void AtualizarMatricula(Matricula matricula);
   void Adicionar(Aluno aluno);
   void AdicionarCertificado(Certificado certificado);
}