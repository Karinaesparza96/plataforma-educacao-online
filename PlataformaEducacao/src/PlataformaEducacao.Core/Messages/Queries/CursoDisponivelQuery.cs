using NetDevPack.SimpleMediator;
using PlataformaEducacao.Core.DomainObjects.DTOs;

namespace PlataformaEducacao.Core.Messages.Queries;

public class CursoDisponivelQuery(Guid cursoId) : IRequest<CursoDto>
{
    public Guid CursoId { get; set; } = cursoId;
}