using CC.Shared.Domain;
using CC.TechSupportService.Domain.Abstractions;
using CC.TechSupportService.Domain.Entities;

namespace CC.TechSupportService.Infrastructure.Persistence.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly DatabaseContext _dbContext;
    
    public TicketRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task AddAsync(Ticket ticket, CancellationToken token)
        => _dbContext.Tickets.AddAsync(ticket, token).AsTask();

    public Task<Ticket?> GetAsync(GuidId id, CancellationToken token)
        => _dbContext.Tickets.FindAsync(new object?[] { id }, token).AsTask();

    public Task RemoveAsync(Ticket ticket, CancellationToken token)
    {
        _dbContext.Tickets.Remove(ticket);
        
        return Task.CompletedTask;
    }
}