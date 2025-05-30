﻿using MediatR;
using PlataformaEducacao.Core.DomainObjects;
using PlataformaEducacao.Pagamentos.Data.Context;

namespace PlataformaEducacao.Pagamentos.Data;

public static class MediatorExtension
{
    public static async Task PublishDomainEvents(this IMediator mediator, PagamentoContext ctx)
    {
        var domainEntities = ctx.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.Notificacoes != null && x.Entity.Notificacoes.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.Notificacoes)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.LimparEventos());

        var tasks = domainEvents
            .Select(async (domainEvent) => {
                await mediator.Publish(domainEvent);
            });

        await Task.WhenAll(tasks);
    }
}