using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaEducacao.Api.Controllers.Base;
using PlataformaEducacao.Api.DTOs;
using PlataformaEducacao.Core.Messages.Notifications;
using PlataformaEducacao.GestaoAlunos.Aplication.Commands;
using PlataformaEducacao.GestaoConteudos.Aplication.Commands;
using PlataformaEducacao.GestaoConteudos.Aplication.Queries;
using PlataformaEducacao.GestaoConteudos.Aplication.Queries.ViewModels;
using System.Net;
using PlataformaEducacao.Core.DomainObjects;

namespace PlataformaEducacao.Api.Controllers
{
    [Route("api/cursos")]
    public class CursosController(INotificationHandler<DomainNotification> notificacoes,
                            IMediator mediator,
                            IAppIdentityUser identityUser,
                            ICursoQueries cursoQueries) : MainController(notificacoes, mediator, identityUser)
    {
        private readonly IMediator _mediator = mediator;

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CursoViewModel>>> ObterTodos()
        {
            var cursos = await cursoQueries.ObterTodos();
            return RespostaPadrao(HttpStatusCode.OK, cursos);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CursoViewModel>> ObterPorId(Guid id)
        {
            var curso = await cursoQueries.ObterPorId(id);
            return RespostaPadrao(HttpStatusCode.OK, curso);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] CursoDto curso)
        {
            var command = new AdicionarCursoCommand(curso.Nome, curso.Conteudo, UsuarioId, curso.Preco);
            await _mediator.Send(command);

            return RespostaPadrao(HttpStatusCode.Created);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] CursoDto curso)
        {
            if (id != curso.Id)
            {
                NotificarErro("Curso", "O ID do curso não pode ser diferente do ID informado na URL.");
                return RespostaPadrao();
            }
            var command = new AtualizarCursoCommand(curso.Id, curso.Nome, curso.Conteudo, curso.Preco);

            await _mediator.Send(command);
            return RespostaPadrao(HttpStatusCode.NoContent);
        }

        [Authorize(Roles = "ALUNO")]
        [HttpPost("{id:guid}/concluir-curso")]
        public async Task<IActionResult> ConcluirCurso(Guid id)
        {
            var command = new ConcluirMatriculaCommand(UsuarioId, id);
            await _mediator.Send(command);

            return RespostaPadrao(HttpStatusCode.Created);
        }

        [Authorize(Roles = "ALUNO")]
        [HttpPost("{cursoId:guid}/realizar-pagamento")]
        public async Task<IActionResult> RealizarPagamento(Guid cursoId, [FromBody] DadosPagamento dadosPagamento)
        {
            var command = new ValidarPagamentoCursoCommand(cursoId, UsuarioId, dadosPagamento.NomeCartao,
                                                           dadosPagamento.NumeroCartao, dadosPagamento.ExpiracaoCartao,
                                                           dadosPagamento.CvvCartao);
            await _mediator.Send(command);

            return RespostaPadrao(HttpStatusCode.Created);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            var command = new DeletarCursoCommand(id);
            await _mediator.Send(command);
            return RespostaPadrao(HttpStatusCode.NoContent);
        }
    }
}
