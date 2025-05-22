using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlataformaEducacao.Api.Controllers.Base;
using PlataformaEducacao.Api.DTOs;
using PlataformaEducacao.Core.Messages;
using PlataformaEducacao.GestaoAlunos.Aplication.Commands;
using PlataformaEducacao.GestaoAlunos.Aplication.Queries;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace PlataformaEducacao.Api.Controllers;

[Authorize(Roles = "ADMIN,ALUNO")]
[Route("api/matriculas")]
public class MatriculasController(INotificationHandler<DomainNotification> notificacoes,
                                 IAlunoQueries alunoQueries,
                                 IMediator mediator) : MainController(notificacoes, mediator)
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
        var command = new AdicionarMatriculaCommand(UsuarioId, cursoId);
        await _mediator.Send(command);

        return RespostaPadrao(HttpStatusCode.Created);
    }
}