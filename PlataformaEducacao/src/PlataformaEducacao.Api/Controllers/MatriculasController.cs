using System.Net;
using Microsoft.AspNetCore.Mvc;
using NetDevPack.SimpleMediator;
using PlataformaEducacao.Api.Controllers.Base;
using PlataformaEducacao.Core.Messages;
using PlataformaEducacao.GestaoAlunos.Aplication.Commands;
using PlataformaEducacao.GestaoConteudos.Aplication.Queries;

namespace PlataformaEducacao.Api.Controllers;

[Route("api/matriculas")]
public class MatriculasController(INotificationHandler<DomainNotification> notificacoes,
                                 ICursoQueries cursoQueries,
                                 IMediator mediator) : MainController(notificacoes, mediator)
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Adicionar([FromBody] CriarMatriculaCommand matriculaCommand)
    {   
        var curso = await cursoQueries.ObterPorId(matriculaCommand.CursoId);

        if (curso == null)
        {
            NotificarErro(matriculaCommand.MessageType, "Curso não encontrado.");
        }
        else
        {
            var command = new CriarMatriculaCommand(matriculaCommand.AlunoId, matriculaCommand.CursoId);
            await _mediator.Send(command);
        }
        
        return RespostaPadrao(HttpStatusCode.Created);
    }
}