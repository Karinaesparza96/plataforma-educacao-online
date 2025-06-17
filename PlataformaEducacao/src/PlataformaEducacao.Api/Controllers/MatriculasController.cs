using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlataformaEducacao.Api.Controllers.Base;
using PlataformaEducacao.Api.DTOs;
using PlataformaEducacao.GestaoAlunos.Aplication.Commands;
using PlataformaEducacao.GestaoAlunos.Aplication.Queries;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using PlataformaEducacao.Core.Messages.Notifications;
using PlataformaEducacao.Core.DomainObjects;
using PlataformaEducacao.GestaoConteudos.Aplication.Queries;

namespace PlataformaEducacao.Api.Controllers;

[Authorize(Roles = "ADMIN,ALUNO")]
[Route("api/matriculas")]
public class MatriculasController(INotificationHandler<DomainNotification> notificacoes,
                                 IAlunoQueries alunoQueries,
                                 IAppIdentityUser identityUser,
                                 ICursoQueries cursoQueries,
                                 IMediator mediator) : MainController(notificacoes, mediator, identityUser)
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("pendentes-pagamento")]
    public async Task<ActionResult<IEnumerable<MatriculaDto>>> ObterMatriculasPendentePagamento()
    {
        var matriculas = await alunoQueries.ObterMatriculasPendentePagamento(UsuarioId);
        return RespostaPadrao(HttpStatusCode.OK, matriculas);
    }

    [HttpPost("{cursoId:guid}")]
    public async Task<IActionResult> Adicionar(Guid cursoId)
    {   
        var curso = await cursoQueries.ObterPorId(cursoId);

        if (curso is null)
        {
            NotificarErro("Curso", "Curso não encontrado.");
            return RespostaPadrao();
        }
        var command = new AdicionarMatriculaCommand(UsuarioId, cursoId);
        await _mediator.Send(command);

        return RespostaPadrao(HttpStatusCode.Created);
    }
}