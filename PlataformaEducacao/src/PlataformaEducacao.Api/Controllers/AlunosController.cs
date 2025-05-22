using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaEducacao.Api.Controllers.Base;
using PlataformaEducacao.Core.Messages;
using PlataformaEducacao.GestaoAlunos.Aplication.Queries;

namespace PlataformaEducacao.Api.Controllers;

[Route("api/alunos")]
public class AlunosController(INotificationHandler<DomainNotification> notificacoes,
                            IAlunoQueries alunoQueries, 
                             IMediator mediator) : MainController(notificacoes, mediator)
{

    [Authorize(Roles = "ALUNO")]
    [HttpGet("certificados/{id:guid}/download")]
    public async Task<IActionResult> BaixarCertificado(Guid id)
    {
        var certificado = await alunoQueries.ObterCertificado(id, UsuarioId);
        if (certificado?.Arquivo == null)
        {
            return NotFound();
        }

        return File(certificado.Arquivo, "application/pdf", "certificado.pdf");
    }
}