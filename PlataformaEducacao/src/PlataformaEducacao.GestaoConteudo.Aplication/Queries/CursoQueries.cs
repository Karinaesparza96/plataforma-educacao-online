using PlataformaEducacao.GestaoConteudos.Aplication.Queries.ViewModels;
using PlataformaEducacao.GestaoConteudos.Domain;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Queries;

public class CursoQueries(ICursoRepository cursoRepository) : ICursoQueries
{
    public async Task<CursoViewModel?> ObterPorId(Guid cursoId)
    {
        var curso = await cursoRepository.ObterPorId(cursoId);

        if (curso is null)
            return null;

        return new CursoViewModel
        {
            Id = curso.Id,
            Nome = curso.Nome,
            ConteudoProgramatico = curso.ConteudoProgramatico,
            Preco = curso.Preco,
            Aulas = curso.Aulas?.Select(a => new AulaViewModel
            {
                Id = a.Id,
                Nome = a.Nome,
                Conteudo = a.Conteudo
            }).ToList() ?? []
        };
    }

    public async Task<IEnumerable<CursoViewModel>> ObterTodos()
    {
        var cursos = await cursoRepository.ObterTodos(); 

        return cursos.Select(c => new CursoViewModel
        {
            Id = c.Id,
            Nome = c.Nome,
            ConteudoProgramatico = c.ConteudoProgramatico,
            Preco = c.Preco,
            Aulas = c.Aulas?.Select(a => new AulaViewModel
            {
                Id = a.Id,
                Nome = a.Nome,
                Conteudo = a.Conteudo
            }).ToList() ?? []
        }).ToList();
    }

    public async Task<bool> TodasAulasConcluidas(Guid cursoId, Guid alunoId)
    {
        var aulas = await cursoRepository.ObterAulasComProgressoFiltradoAluno(cursoId, alunoId);

        return aulas.All(a => a.EstaConcluida(alunoId));
    }
}