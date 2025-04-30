using System.Net;
using Microsoft.AspNetCore.Mvc;
using NetDevPack.SimpleMediator;
using PlataformaEducacao.Api.Controllers.Base;
using PlataformaEducacao.Core.Messages;

namespace PlataformaEducacao.Api.Controllers;

[Route("api/alunos")]
public class AlunosController(INotificationHandler<DomainNotification> notificacoes, 
                             IMediator mediator) : MainController(notificacoes, mediator)
{
    private readonly IMediator _mediator = mediator;
}