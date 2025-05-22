using MediatR;
using PlataformaEducacao.Core.DomainObjects.DTOs;

namespace PlataformaEducacao.Core.Messages.IntegrationQueries;

public class ObterCursoQuery(Guid cursoId) : IRequest<CursoDto?>
{
    public Guid CursoId { get; set; } = cursoId;
}