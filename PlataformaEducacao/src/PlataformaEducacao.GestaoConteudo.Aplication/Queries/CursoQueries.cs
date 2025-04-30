using PlataformaEducacao.GestaoConteudos.Aplication.Queries.ViewModels;
using PlataformaEducacao.GestaoConteudos.Domain;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Queries;

public class CursoQueries : ICursoQueries
{
    private readonly ICursoRepository _cursoRepository;

    public CursoQueries(ICursoRepository cursoRepository)
    {
        _cursoRepository = cursoRepository;
    }
    public async Task<CursoViewModel?> ObterCursoPorAulaId(Guid aulaId)
    {
        var curso = await _cursoRepository.ObterCursoPorAulaId(aulaId);

        if (curso == null) 
            return null;

        return new CursoViewModel
        {
            Id = curso.Id,
            Nome = curso.Nome,
            ConteudoProgramatico = curso.ConteudoProgramatico,
            Aulas = curso.Aulas.Select(a => new AulaViewModel
            {
                Id = a.Id,
                Nome = a.Nome,
                Conteudo = a.Conteudo
            })
        };
    }

    public async Task<CursoViewModel?> ObterPorId(Guid cursoId)
    {
        var curso = await _cursoRepository.ObterPorId(cursoId);

        if (curso == null) 
            return null;

        return new CursoViewModel
        {
            Id = curso.Id,
            Nome = curso.Nome,
            ConteudoProgramatico = curso.ConteudoProgramatico,
            Aulas = curso.Aulas.Select(a => new AulaViewModel
            {
                Id = a.Id,
                Nome = a.Nome,
                Conteudo = a.Conteudo
            }).ToList()
        };
    }

    public async Task<IEnumerable<CursoViewModel>> ObterTodos()
    {
        var cursos = await _cursoRepository.ObterTodos(); 

        return cursos.Select(c => new CursoViewModel
        {
            Id = c.Id,
            Nome = c.Nome,
            ConteudoProgramatico = c.ConteudoProgramatico,
            Aulas = c.Aulas.Select(a => new AulaViewModel
            {
                Id = a.Id,
                Nome = a.Nome,
                Conteudo = a.Conteudo
            }).ToList()
        }).ToList();
    }
}