using PlataformaEducacao.Core.Data;

namespace PlataformaEducacao.GestaoConteudos.Domain;

public interface ICursoRepository : IRepository<Curso>
{
    Task<Curso?> ObterPorId(Guid cursoId);
    Task<Curso?> ObterCursoComAulas(Guid cursoId);
    Task<IEnumerable<Curso>> ObterTodos();
    Task<Curso?> ObterCursoPorAulaId(Guid aulaId);
    Task<Aula?> ObterAulaPorId(Guid aulaId);

    Task<IEnumerable<Aula>> ObterAulasComProgressoFiltradoAluno(Guid cursoId, Guid alunoId);
    void Adicionar(Curso curso);
    
    void Adicionar(Aula aula);
    void Atualizar(Curso curso);
    void Remover(Curso curso);
}