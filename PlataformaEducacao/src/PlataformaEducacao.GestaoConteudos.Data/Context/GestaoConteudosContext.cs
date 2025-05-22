using MediatR;
using Microsoft.EntityFrameworkCore;
using PlataformaEducacao.Core.Data;
using PlataformaEducacao.Core.DomainObjects;
using PlataformaEducacao.Core.Messages;
using System.Linq.Expressions;
using System.Reflection.Emit;

namespace PlataformaEducacao.GestaoConteudos.Data.Context;

public class GestaoConteudosContext(DbContextOptions<GestaoConteudosContext> options,
                                    IMediator mediator) : DbContext(options), IUnitOfWork
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        foreach (var property in builder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetProperties()
                         .Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("varchar(200)");

        builder.ApplyConfigurationsFromAssembly(typeof(GestaoConteudosContext).Assembly);

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(Entity).IsAssignableFrom(entityType.ClrType) &&
                !entityType.ClrType.IsAbstract && entityType.BaseType == null)
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, nameof(Entity.DataExclusao));
                var condition = Expression.Equal(property, Expression.Constant(null));
                var lambda = Expression.Lambda(condition, parameter);

                builder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }

        foreach (var relationShip in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationShip.DeleteBehavior = DeleteBehavior.ClientSetNull;
        }

        builder.Ignore<Event>();

        base.OnModelCreating(builder);
    }

    public async Task<bool> Commit()
    {   
        var sucesso = await SaveChangesAsync() > 0;
        
        if (sucesso)
            await mediator.PublishDomainEvents(this);

        return sucesso;
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entityEntry in ChangeTracker.Entries<Entity>())
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property("DataCriacao").CurrentValue = DateTime.Now;
                entityEntry.Property("DataAlteracao").IsModified = false;
                entityEntry.Property("DataExclusao").IsModified = false;
            }
            if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property("DataAlteracao").CurrentValue = DateTime.Now;
                entityEntry.Property("DataCriacao").IsModified = false;
                entityEntry.Property("DataExclusao").IsModified = false;
            }
            if (entityEntry.State == EntityState.Deleted)
            {
                entityEntry.State = EntityState.Modified;
                entityEntry.Property("DataExclusao").CurrentValue = DateTime.Now;
                entityEntry.Property("DataCriacao").IsModified = false;
                entityEntry.Property("DataAlteracao").IsModified = false;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}