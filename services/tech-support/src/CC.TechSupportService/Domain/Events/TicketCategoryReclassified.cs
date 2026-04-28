using CC.Shared.Domain;
using CC.TechSupportService.Domain.ValueObjects.Ticket;

namespace CC.TechSupportService.Domain.Events;

public record TicketCategoryReclassified(Guid TicketId, 
    TicketCategory From, 
    TicketCategory To, 
    Guid ReclassifiedBy, 
    DateTime ReclassifiedAt) : IDomainEvent;