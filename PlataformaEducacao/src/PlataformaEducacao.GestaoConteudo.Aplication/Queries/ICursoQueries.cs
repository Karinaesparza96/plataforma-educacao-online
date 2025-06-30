using PlataformaEducacao.GestaoConteudos.Aplication.Queries.ViewModels;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Queries;

public interface ICursoQueries
{
    Task<CursoViewModel?> ObterPorId(Guid cursoId);
    Task<IEnumerable<CursoViewModel>> ObterTodos();
    Task<HistoricoAprendizagemCursoViewModel?> ObterHistoricoAprendizagem(Guid cursoId, Guid usuarioId);
}