using PlataformaEducacao.Core.Data;

namespace PlataformaEducacao.GestaoConteudos.Domain;

public interface ICursoRepository : IRepository<Curso>
{
    Task<Curso?> ObterPorId(Guid cursoId);
    Task<Curso?> ObterCursoComAulas(Guid cursoId);
    Task<IEnumerable<Curso>> ObterTodos();
    Task<Aula?> ObterAulaPorId(Guid aulaId);
    Task<ProgressoCurso?> ObterProgressoCurso(Guid cursoId, Guid alunoId);

    void Adicionar(Curso curso);
    void Adicionar(Aula aula);
    void Adicionar(ProgressoCurso progressoCurso);
    void Atualizar(ProgressoCurso progressoCurso);
    void Atualizar(Curso curso);
    void Remover(Curso curso);
}