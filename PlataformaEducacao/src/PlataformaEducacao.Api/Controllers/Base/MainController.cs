using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
using PlataformaEducacao.Core.Messages.Notifications;

namespace PlataformaEducacao.Api.Controllers.Base
{
    [ApiController]
    public abstract class MainController(
        INotificationHandler<DomainNotification> notificacoes,
        IMediator mediator)
        : ControllerBase
    {
        private readonly DomainNotificationHandler _notificacoes = (DomainNotificationHandler)notificacoes;

        protected Guid UsuarioId => Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty);

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
            mediator.Publish(new DomainNotification(codigo, mensagem));
        }
        protected void NotificarErro(ModelStateDictionary modelState)
        {
            foreach (var error in modelState.Values.SelectMany(v => v.Errors))
            {
                NotificarErro("ModelState", error.ErrorMessage);
            }
        }
        protected void NotificarErro(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                NotificarErro("Identity", error.Description);
            }
        }
    }
}
