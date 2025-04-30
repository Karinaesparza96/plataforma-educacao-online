using Microsoft.AspNetCore.Mvc;
using NetDevPack.SimpleMediator;
using PlataformaEducacao.Api.Controllers.Base;
using PlataformaEducacao.Core.Messages;
using PlataformaEducacao.GestaoAlunos.Aplication.Queries;
using PlataformaEducacao.GestaoAlunos.Domain;
using PlataformaEducacao.GestaoConteudos.Aplication.Commands;
using System.Net;

namespace PlataformaEducacao.Api.Controllers;

[Route("api/aulas")]
public class AulasController(INotificationHandler<DomainNotification> notificacoes,
                            IAlunoQueries alunoQueries,
                            IMediator mediator) : MainController(notificacoes, mediator)
{
    private readonly IMediator _mediator = mediator;
    private readonly IAlunoQueries _alunoQueries = alunoQueries;

    [HttpPost]
    public async Task<IActionResult> Adicionar([FromBody] AdicionarAulaCommand aulaCommand)
    {
        var command = new AdicionarAulaCommand(aulaCommand.Nome, aulaCommand.Conteudo, aulaCommand.CursoId,
                                               aulaCommand.NomeMaterial, aulaCommand.TipoMaterial);

        await _mediator.Send(command);

        return RespostaPadrao(HttpStatusCode.Created);
    }

    [HttpPost("realizar-aula")]
    public async Task<IActionResult> Realizar([FromBody] RealizarAulaCommand realizarAulaCommand)
    {
        var matricula = await _alunoQueries.ObterMatriculaPorAlunoECursoId(realizarAulaCommand.CursoId,
            realizarAulaCommand.AlunoId);

        if (matricula?.Status != EStatusMatricula.Ativa)
        {
            NotificarErro(realizarAulaCommand.MessageType, "Matrícula não encontrada ou não está ativa.");
        }
        else
        {
            var command = new RealizarAulaCommand(realizarAulaCommand.AulaId, realizarAulaCommand.AlunoId, realizarAulaCommand.CursoId);
            await _mediator.Send(command);
        }

        return RespostaPadrao(HttpStatusCode.Created);
    }

}