using CC.Shared.Domain;

namespace CC.TechSupportService.Domain.Events;

public sealed record AssigneeChanged(Guid OldAssigneeId, Guid NewAssigneeId) : IDomainEvent;