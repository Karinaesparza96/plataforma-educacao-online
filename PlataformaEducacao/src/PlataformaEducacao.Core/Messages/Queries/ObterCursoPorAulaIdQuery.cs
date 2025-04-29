using NetDevPack.SimpleMediator;
using PlataformaEducacao.Core.DomainObjects.DTOs;

namespace PlataformaEducacao.Core.Messages.Queries;

public class ObterCursoPorAulaIdQuery(Guid aulaId) : IRequest<CursoDto>
{
    public Guid AulaId { get; set; } = aulaId;
}