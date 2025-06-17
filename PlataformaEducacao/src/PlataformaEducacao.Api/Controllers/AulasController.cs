using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaEducacao.Api.Controllers.Base;
using PlataformaEducacao.Api.DTOs;
using PlataformaEducacao.Core.DomainObjects;
using PlataformaEducacao.Core.DomainObjects.Enums;
using PlataformaEducacao.Core.Messages.Notifications;
using PlataformaEducacao.GestaoAlunos.Aplication.Queries;
using PlataformaEducacao.GestaoAlunos.Aplication.Queries.ViewModels;
using PlataformaEducacao.GestaoConteudos.Aplication.Commands;
using System.Net;

namespace PlataformaEducacao.Api.Controllers;

[Route("api/cursos/{cursoId:guid}/aulas")]
public class AulasController(INotificationHandler<DomainNotification> notificacoes,
                            IAppIdentityUser identityUser,
                            IAlunoQueries alunoQueries,
                            IMediator mediator) : MainController(notificacoes, mediator, identityUser)
{
    private readonly IMediator _mediator = mediator;

    [Authorize(Roles = "ADMIN")]
    [HttpPost("adicionar-aula")]
    public async Task<IActionResult> Adicionar([FromBody] AulaDto aulaDto, Guid cursoId)
    {
        var command = new AdicionarAulaCommand(aulaDto.Nome, aulaDto.Conteudo, cursoId,
                                               aulaDto.NomeMaterial, aulaDto.TipoMaterial);

        await _mediator.Send(command);

        return RespostaPadrao(HttpStatusCode.Created);
    }

    [Authorize(Roles = "ALUNO")]
    [HttpPost("{id:guid}/realizar-aula")]
    public async Task<IActionResult> Realizar(Guid id, Guid cursoId)
    {
        var matricula = await alunoQueries.ObterMatricula(cursoId, UsuarioId);

        ValidarMatricula(matricula);

        if (!OperacaoValida())
            return RespostaPadrao();

        var command = new RealizarAulaCommand(id, UsuarioId, cursoId);
        await _mediator.Send(command);

        return RespostaPadrao(HttpStatusCode.Created);
    }

    [Authorize(Roles = "ALUNO")]
    [HttpPost("{id:guid}/concluir-aula")]
    public async Task<IActionResult> Concluir(Guid id, Guid cursoId)
    {
        var matricula = await alunoQueries.ObterMatricula(cursoId, UsuarioId);

        ValidarMatricula(matricula);

        if (!OperacaoValida())
            return RespostaPadrao();

        var command = new ConcluirAulaCommand(id, UsuarioId, cursoId);
        await _mediator.Send(command);
        
        return RespostaPadrao(HttpStatusCode.Created);
    }

    private void ValidarMatricula(MatriculaViewModel? matricula)
    {
        if (matricula is null)
        {
            NotificarErro("Matricula", "Matrícula não encontrada.");
            return;
        }

        if (matricula?.Status != EStatusMatricula.Ativa)
        {
            NotificarErro("Matricula", "Matrícula não está ativa.");
        }
    }
}