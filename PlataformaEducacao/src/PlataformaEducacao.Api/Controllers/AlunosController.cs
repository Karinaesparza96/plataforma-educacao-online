using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaEducacao.Api.Controllers.Base;
using PlataformaEducacao.Core.DomainObjects;
using PlataformaEducacao.Core.Messages.Notifications;
using PlataformaEducacao.GestaoAlunos.Aplication.Queries;

namespace PlataformaEducacao.Api.Controllers;

[Route("api/alunos")]
public class AlunosController(INotificationHandler<DomainNotification> notificacoes,
                            IAlunoQueries alunoQueries,
                            IAppIdentityUser identityUser,
                             IMediator mediator) : MainController(notificacoes, mediator, identityUser)
{

    [Authorize(Roles = "ALUNO")]
    [HttpGet("certificados/{id:guid}/download")]
    public async Task<IActionResult> BaixarCertificado(Guid id)
    {
        var certificado = await alunoQueries.ObterCertificado(id, UsuarioId);
        if (certificado?.Arquivo == null)
        {
            return NoContent();
        }

        return File(certificado.Arquivo, "application/pdf", "certificado.pdf");
    }
}