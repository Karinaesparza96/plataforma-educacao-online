using Microsoft.EntityFrameworkCore;
using NetDevPack.SimpleMediator;
using PlataformaEducacao.Core.Data;
using PlataformaEducacao.Core.DomainObjects;

namespace PlataformaEducacao.GestaoConteudos.Data.Context;

public class GestaoConteudosContext(DbContextOptions<GestaoConteudosContext> options,
                                    IMediator mediator) : DbContext(options), IUnitOfWork
{   
    private readonly IMediator _mediator = mediator;

    public async Task<bool> Commit()
    {   
        var sucesso = await base.SaveChangesAsync() > 0;
        
        if (sucesso)
            await _mediator.PublishDomainEvents(this);

        return sucesso;
    }
}

public static class MediatorExtensions
{
    public static async Task PublishDomainEvents(this IMediator mediator, DbContext context)
    {
        var domainEntities = context.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.Notificacoes != null && x.Entity.Notificacoes.Any())
            .Select(x => x.Entity)
            .ToList();

        var domainEvents = domainEntities.SelectMany(x => x.Notificacoes).ToList();

        domainEntities.ForEach(entity => entity.LimparEventos());

        var tasks = domainEvents.Select(async (domainEvent) =>
        {
            await mediator.Publish(domainEvent);
        });

        await Task.WhenAll(tasks);
    }
}