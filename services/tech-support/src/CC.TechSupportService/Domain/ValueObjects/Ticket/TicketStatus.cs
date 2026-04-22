namespace CC.TechSupportService.Domain.ValueObjects.Ticket;

public abstract record TicketStatus(string Value)
{
    public abstract bool CanTransitionTo(TicketStatus newStatus);
}

public record New() : TicketStatus(nameof(New))
{
    public override bool CanTransitionTo(TicketStatus newStatus)
        => newStatus is Opened;
}

public record Opened() : TicketStatus(nameof(Opened))
{
    public override bool CanTransitionTo(TicketStatus newStatus)
        => newStatus is InProgress or Escalated or Closed;
}

public record InProgress() : TicketStatus(nameof(InProgress))
{
    public override bool CanTransitionTo(TicketStatus newStatus)
        => newStatus is WaitingForUser or Resolved or Escalated;
}

public record WaitingForUser() : TicketStatus(nameof(WaitingForUser))
{
    public override bool CanTransitionTo(TicketStatus newStatus)
        => newStatus is InProgress or Resolved;
}

public record Escalated() : TicketStatus(nameof(Escalated))
{
    public override bool CanTransitionTo(TicketStatus newStatus)
        => newStatus is InProgress or Resolved;
}

public record Resolved(DateTime ResolvedAt) : TicketStatus(nameof(Resolved))
{
    public override bool CanTransitionTo(TicketStatus newStatus)
        => newStatus is Closed or Reopened;
}

public record Reopened() : TicketStatus(nameof(Reopened))
{
    public override bool CanTransitionTo(TicketStatus newStatus)
        => newStatus is InProgress or Escalated;
}

public record Closed(DateTime ClosedAt) : TicketStatus(nameof(Closed))
{
    public override bool CanTransitionTo(TicketStatus newStatus)
        => newStatus is Reopened;
}