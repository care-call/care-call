using CC.Shared.Domain;
using CC.TechSupportService.Domain.Entities;

namespace CC.TechSupportService.Domain.Abstractions;

public interface ITicketRepository
{
    Task AddAsync(Ticket ticket, CancellationToken token = default);

    Task<Ticket?> GetAsync(GuidId id, CancellationToken token = default);

    Task RemoveAsync(Ticket ticket, CancellationToken token = default);
}