using CC.TechSupportService.Domain.Entities;

namespace CC.TechSupportService.Domain.Abstractions;

public interface ITicketRepository
{
    ValueTask AddAsync(Ticket ticket, CancellationToken token);
    ValueTask<Ticket?> GetAsync(Guid id, CancellationToken token);
    void Remove(Ticket ticket);
}