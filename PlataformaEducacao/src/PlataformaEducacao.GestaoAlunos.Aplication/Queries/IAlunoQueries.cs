using PlataformaEducacao.GestaoAlunos.Aplication.Queries.ViewModels;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Queries;

public interface IAlunoQueries
{
    Task<MatriculaViewModel?> ObterMatricula(Guid cursoId, Guid alunoId);
    Task<IEnumerable<MatriculaViewModel>> ObterMatriculasPendentePagamento(Guid alunoId);
    Task<CertificadoViewModel?> ObterCertificado(Guid certificadoId, Guid alunoId);
}