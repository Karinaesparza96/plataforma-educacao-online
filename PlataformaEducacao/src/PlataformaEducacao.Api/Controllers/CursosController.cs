using System.Net;
using Microsoft.AspNetCore.Mvc;
using NetDevPack.SimpleMediator;
using PlataformaEducacao.Api.Controllers.Base;
using PlataformaEducacao.Core.Messages;
using PlataformaEducacao.GestaoConteudos.Aplication.Commands;
using PlataformaEducacao.GestaoConteudos.Aplication.Queries;
using PlataformaEducacao.GestaoConteudos.Aplication.Queries.ViewModels;

namespace PlataformaEducacao.Api.Controllers
{
    [Route("api/cursos")]
    public class CursosController(INotificationHandler<DomainNotification> notificacoes,
                                  IMediator mediator, ICursoQueries cursoQueries) 
        : MainController(notificacoes, mediator)
    {   
        private readonly IMediator _mediator = mediator;
        private readonly ICursoQueries _cursoQueries = cursoQueries;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CursoViewModel>>> ObterTodos()
        {
            var cursos = await _cursoQueries.ObterTodos();
            return RespostaPadrao(HttpStatusCode.OK, cursos);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CursoViewModel>> ObterPorId(Guid id)
        {
            var curso = await _cursoQueries.ObterPorId(id);
            return RespostaPadrao(HttpStatusCode.OK, curso);
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] AdicionarCursoCommand cursoCommand)
        {
            var command = new AdicionarCursoCommand(cursoCommand.Nome, cursoCommand.ConteudoProgramatico, UsuarioId);

            await _mediator.Send(command);

            return RespostaPadrao(HttpStatusCode.Created);
        }
    }
}
