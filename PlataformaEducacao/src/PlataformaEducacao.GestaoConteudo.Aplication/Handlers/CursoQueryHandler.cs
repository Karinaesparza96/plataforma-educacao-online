using NetDevPack.SimpleMediator;
using PlataformaEducacao.Core.DomainObjects.DTOs;
using PlataformaEducacao.Core.Messages.Queries;
using PlataformaEducacao.GestaoConteudos.Domain;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Handlers;

public class CursoQueryHandler(ICursoRepository cursoRepository) : IRequestHandler<ObterCursoPorAulaIdQuery, CursoDto>, IRequestHandler<CursoDisponivelQuery, CursoDto>
{
    private readonly ICursoRepository _cursoRepository = cursoRepository;
    public async Task<CursoDto> Handle(ObterCursoPorAulaIdQuery request, CancellationToken cancellationToken)
    {
        var curso = await _cursoRepository.ObterCursoPorAulaId(request.AulaId);
        return curso?.ToDto();
    }

    public async Task<CursoDto> Handle(CursoDisponivelQuery request, CancellationToken cancellationToken)
    {
        var curso = await _cursoRepository.ObterPorId(request.CursoId);
        return curso?.ToDto();
    }
}