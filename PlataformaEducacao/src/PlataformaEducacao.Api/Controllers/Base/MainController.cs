using Microsoft.AspNetCore.Mvc;
using NetDevPack.SimpleMediator;
using PlataformaEducacao.Core.Messages;
using System.Net;

namespace PlataformaEducacao.Api.Controllers.Base
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        private readonly DomainNotificationHandler _notificacoes;
        private readonly IMediator _mediator;

        protected Guid UsuarioId => Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value ?? string.Empty);
        protected MainController(INotificationHandler<DomainNotification> notificacoes, 
                                IMediator mediator)
        {
            _notificacoes = (DomainNotificationHandler)notificacoes; ;
            _mediator = mediator;
        }
        protected bool OperacaoValida()
        {
            return !_notificacoes.TemNotificacao();
        }
        protected ActionResult RespostaPadrao(HttpStatusCode statusCode = HttpStatusCode.OK, object? data = null)
        {
            if (OperacaoValida())
            {
                return new ObjectResult(new
                {
                    Sucesso = true,
                    Data = data,
                })
                {
                    StatusCode = (int)statusCode
                };
            }
                
            return BadRequest(new
            {
                Sucesso = false,
                Erros = _notificacoes.ObterNotificacoes().Select(n => n.Value)
            });
        }
        protected void NotificarErro(string codigo, string mensagem)
        {
            _mediator.Publish(new DomainNotification(codigo, mensagem));
        }
    }
}
