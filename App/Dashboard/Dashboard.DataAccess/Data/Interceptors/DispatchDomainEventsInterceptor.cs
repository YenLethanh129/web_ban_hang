using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Dashboard.DataAccess.Models.Entities.Base;

namespace Dashboard.DataAccess.Data.Interceptors;
public class DispatchDomainEventsInterceptor(IMediator mediator) : SaveChangesInterceptor
{
	private readonly IMediator _mediator = mediator;

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result, 
        CancellationToken cancellationToken = default)
    {
        await DispatchDomainEvents(eventData.Context, cancellationToken);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

	public async Task DispatchDomainEvents(DbContext? context, CancellationToken cancellationToken = default)
	{
		if (context == null) return;

        var entitiesWithEvents = context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();


        var domainEvents = entitiesWithEvents
            .SelectMany(e => e.DomainEvents)
            .ToList();


        entitiesWithEvents.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }
    }
}