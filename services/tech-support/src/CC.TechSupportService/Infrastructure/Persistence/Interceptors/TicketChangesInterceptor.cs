using CC.TechSupportService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using StackExchange.Redis;

namespace CC.TechSupportService.Infrastructure.Persistence.Interceptors;

public class TicketChangesInterceptor(IConnectionMultiplexer connectionMultiplexer) : SaveChangesInterceptor
{
    private List<Ticket> _addedTickets = [];

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        _addedTickets = eventData.Context?.ChangeTracker
            .Entries<Ticket>()
            .Where(x => x.State == EntityState.Added)
            .Select(x => x.Entity)
            .ToList() ?? [];

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override async Task SaveChangesFailedAsync(
        DbContextErrorEventData eventData,
        CancellationToken cancellationToken = default)
    {
        var db = connectionMultiplexer.GetDatabase();
        foreach (var ticket in _addedTickets)
        {
            var key = $"tickets:{ticket.Reporter.ReporterId}";
            await db.StringDecrementAsync(key);
        }
    }
}