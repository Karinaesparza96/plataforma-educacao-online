using Microsoft.AspNetCore.Mvc;
using MediatR;
using PlataformaEducacao.Api.Controllers.Base;
using PlataformaEducacao.Core.Messages;
using PlataformaEducacao.GestaoConteudos.Aplication.Commands;
using System.Net;
using PlataformaEducacao.Api.DTOs;

namespace PlataformaEducacao.Api.Controllers;

[Route("api/cursos/{cursoId:guid}/aulas")]
public class AulasController(INotificationHandler<DomainNotification> notificacoes,
                            IMediator mediator) : MainController(notificacoes, mediator)
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("adicionar-aula")]
    public async Task<IActionResult> Adicionar([FromBody] AulaDto aulaDto, Guid cursoId)
    {
        var command = new AdicionarAulaCommand(aulaDto.Nome, aulaDto.Conteudo, cursoId,
                                               aulaDto.NomeMaterial, aulaDto.TipoMaterial);

        await _mediator.Send(command);

        return RespostaPadrao(HttpStatusCode.Created);
    }

    [HttpPost("{id:guid}/realizar-aula")]
    public async Task<IActionResult> Realizar(Guid id, Guid cursoId)
    {
        var command = new RealizarAulaCommand(id, UsuarioId, cursoId);
        await _mediator.Send(command);

        return RespostaPadrao(HttpStatusCode.Created);
    }
    [HttpPost("{id:guid}/concluir-aula")]
    public async Task<IActionResult> Concluir(Guid id, Guid cursoId)
    {
        var command = new ConcluirAulaCommand(id, UsuarioId, cursoId);
        await _mediator.Send(command);
        
        return RespostaPadrao(HttpStatusCode.Created);
    }

}