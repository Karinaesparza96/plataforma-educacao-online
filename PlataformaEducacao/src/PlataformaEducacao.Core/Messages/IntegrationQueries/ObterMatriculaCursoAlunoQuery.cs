using MediatR;
using PlataformaEducacao.Core.DomainObjects.DTOs;

namespace PlataformaEducacao.Core.Messages.IntegrationQueries;

public class ObterMatriculaCursoAlunoQuery(Guid cursoId, Guid alunoId) : IRequest<MatriculaDto?>
{
    public Guid CursoId { get; set; } = cursoId;
    public Guid AlunoId { get; set; } = alunoId;
}