using CC.Shared.Domain;
using CC.TechSupportService.Domain.ValueObjects.Ticket;

namespace CC.TechSupportService.Domain.Events;

public record TicketCreated(Guid TicketId) : IDomainEvent;

public record TicketOpened(Guid TicketId, TicketStatus OldStatus) : IDomainEvent;

public record TicketProgressed(Guid TicketId, TicketStatus OldStatus) : IDomainEvent;

public record TicketShiftedToWaitingForUser(Guid TicketId, TicketStatus OldStatus) : IDomainEvent;

public record TicketEscalated(Guid TicketId, TicketStatus OldStatus) : IDomainEvent;

public record TicketResolved(Guid TicketId, TicketStatus OldStatus) : IDomainEvent;

public record TicketReopened(Guid TicketId, TicketStatus OldStatus) : IDomainEvent;

public record TicketClosed(Guid TicketId, TicketStatus OldStatus) : IDomainEvent;