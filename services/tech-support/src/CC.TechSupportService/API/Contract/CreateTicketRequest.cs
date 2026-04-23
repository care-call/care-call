using CC.Shared.Domain;
using CC.TechSupportService.Domain.ValueObjects.Ticket;

namespace CC.TechSupportService.API.Contract;

public record CreateTicketRequest(GuidId id,
    int number,
    Reporter reporter,
    TicketSubject subject,
    TicketDescription description,
    TicketCategory ticketCategory,
    RelatedEntity? relatedEntity,
    DateTime createdAt);