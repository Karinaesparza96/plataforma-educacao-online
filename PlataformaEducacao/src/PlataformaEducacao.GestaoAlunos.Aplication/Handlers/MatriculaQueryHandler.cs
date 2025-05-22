using MediatR;
using PlataformaEducacao.Core.DomainObjects.DTOs;
using PlataformaEducacao.Core.Messages.IntegrationQueries;
using PlataformaEducacao.GestaoAlunos.Domain;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Handlers;

public class MatriculaQueryHandler(IAlunoRepository alunoRepository) : IRequestHandler<ObterMatriculaCursoAlunoQuery, MatriculaDto?>
{
    public async Task<MatriculaDto?> Handle(ObterMatriculaCursoAlunoQuery request, CancellationToken cancellationToken)
    {
        var matricula = await alunoRepository.ObterMatriculaPorCursoEAlunoId(request.CursoId, request.AlunoId);
        if (matricula == null)
            return null;

        return new MatriculaDto
        {
            Id = matricula.Id,
            Status = matricula.Status
        };
    }
}