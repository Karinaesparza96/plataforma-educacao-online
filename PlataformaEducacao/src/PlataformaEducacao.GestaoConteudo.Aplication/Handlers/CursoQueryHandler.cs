using MediatR;
using PlataformaEducacao.Core.DomainObjects.DTOs;
using PlataformaEducacao.Core.DomainObjects.Enums;
using PlataformaEducacao.Core.Messages.IntegrationQueries;
using PlataformaEducacao.GestaoConteudos.Domain;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Handlers;

public class CursoQueryHandler(ICursoRepository cursoRepository) : IRequestHandler<ObterCursoQuery, CursoDto?>, 
                                                                    IRequestHandler<ObterAulasCursoAlunoQuery, CursoDto>
{
    public async Task<CursoDto?> Handle(ObterCursoQuery request, CancellationToken cancellationToken)
    {
        var curso = await cursoRepository.ObterPorId(request.CursoId);

        if (curso == null) 
            return null;

        return new CursoDto
        {
            Id = curso.Id,
            Nome = curso.Nome,
            Preco = curso.Preco,
        };
    }

    public async Task<CursoDto> Handle(ObterAulasCursoAlunoQuery request, CancellationToken cancellationToken)
    {
        var aulas = await cursoRepository.ObterAulasComProgressoFiltradoAluno(request.CursoId, request.AlunoId);

        return new CursoDto
        {
            Id = request.CursoId,
            Aulas = aulas?.Select(a =>
            {
                var progresso = a.ProgressoAulas.FirstOrDefault();

                return new AulaDto
                {
                    Id = a.Id,
                    CursoId = a.CursoId,
                    Nome = a.Nome,
                    Conteudo = a.Conteudo,
                    Status = progresso?.Status ?? EProgressoAulaStatus.NaoIniciada
                };
            }).ToList() ?? []
        };
    }
}