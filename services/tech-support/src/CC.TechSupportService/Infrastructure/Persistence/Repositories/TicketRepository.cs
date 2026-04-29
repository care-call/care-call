using CC.TechSupportService.Domain.Abstractions;
using CC.TechSupportService.Domain.Entities;

namespace CC.TechSupportService.Infrastructure.Persistence.Repositories;

public class TicketRepository(DatabaseContext dbContext) : ITicketRepository
{
    public async ValueTask AddAsync(Ticket ticket, CancellationToken token)
        => await dbContext.Tickets.AddAsync(ticket, token);

    public async ValueTask<Ticket?> GetAsync(Guid id, CancellationToken token)
        => await dbContext.Tickets.FindAsync([id], token);

    public void Remove(Ticket ticket)
        => dbContext.Tickets.Remove(ticket);
}