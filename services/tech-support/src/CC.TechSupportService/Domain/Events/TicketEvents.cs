using CC.Shared.Domain;
using CC.TechSupportService.Domain.ValueObjects.Ticket;

namespace CC.TechSupportService.Domain.Events;

public sealed record TicketCreated(Guid TicketId) : IDomainEvent;

public sealed record TicketEvents(Guid TicketId, TicketStatus OldStatus) : IDomainEvent;

public sealed record TicketProgressed(Guid TicketId, TicketStatus OldStatus) : IDomainEvent;

public sealed record TicketShiftedToWaitingForUser(Guid TicketId, TicketStatus OldStatus) : IDomainEvent;

public sealed record TicketEscalated(Guid TicketId, TicketStatus OldStatus) : IDomainEvent;

public sealed record TicketResolved(Guid TicketId, TicketStatus OldStatus) : IDomainEvent;

public sealed record TicketReopened(Guid TicketId, TicketStatus OldStatus) : IDomainEvent;

public sealed record TicketClosed(Guid TicketId, TicketStatus OldStatus) : IDomainEvent;