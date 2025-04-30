using PlataformaEducacao.GestaoConteudos.Aplication.Queries.ViewModels;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Queries;

public interface ICursoQueries
{
    Task<CursoViewModel?> ObterCursoPorAulaId(Guid aulaId);
    Task<CursoViewModel?> ObterPorId(Guid cursoId);
    Task<IEnumerable<CursoViewModel>> ObterTodos();
}