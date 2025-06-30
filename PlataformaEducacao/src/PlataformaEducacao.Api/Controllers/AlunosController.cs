using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaEducacao.Api.Controllers.Base;
using PlataformaEducacao.Core.DomainObjects;
using PlataformaEducacao.Core.Messages.Notifications;
using PlataformaEducacao.GestaoAlunos.Aplication.Queries;
using PlataformaEducacao.GestaoConteudos.Aplication.Queries;

namespace PlataformaEducacao.Api.Controllers;

[Route("api/alunos")]
public class AlunosController(INotificationHandler<DomainNotification> notificacoes,
                            IAlunoQueries alunoQueries,
                            ICursoQueries cursoQueries,
                            IAppIdentityUser identityUser,
                             IMediator mediator) : MainController(notificacoes, mediator, identityUser)
{

    [Authorize(Roles = "ALUNO")]
    [HttpGet("certificados/{id:guid}/download")]
    public async Task<IActionResult> BaixarCertificado(Guid id)
    {
        var certificado = await alunoQueries.ObterCertificado(id, UsuarioId);
        if (certificado?.Arquivo == null || certificado.Arquivo.Length == 0)
        {
            return BadRequest();
        }

        return File(certificado.Arquivo, "application/pdf", "certificado.pdf");
    }

    [Authorize(Roles = "ALUNO")]
    [HttpGet("historico-aprendizagem/{cursoId:guid}")]
    public async Task<IActionResult> ObterHistoricoAprendizagem(Guid cursoId)
    {
        var historico = await cursoQueries.ObterHistoricoAprendizagem(cursoId, UsuarioId);

        if (historico == null)
        {   
            NotificarErro("HistoricoAprendizagem", "Não foi encontrado nenhum histórico para o curso informado.");
            return RespostaPadrao();
        }
        return RespostaPadrao(data: historico);
    }
}