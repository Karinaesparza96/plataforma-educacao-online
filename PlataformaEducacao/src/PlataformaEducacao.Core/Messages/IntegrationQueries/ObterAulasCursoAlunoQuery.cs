using MediatR;
using PlataformaEducacao.Core.DomainObjects.DTOs;

namespace PlataformaEducacao.Core.Messages.IntegrationQueries;

public class ObterAulasCursoAlunoQuery(Guid cursoId, Guid alunoId) : IRequest<CursoDto>
{
    public Guid CursoId { get; set; } = cursoId;
    public Guid AlunoId { get; set; } = alunoId;
}