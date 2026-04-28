namespace CC.TechSupportService.Domain.ValueObjects.Ticket;

public abstract record TicketStatus()
{
    public abstract bool CanTransitionTo(TicketStatus newStatus);
}

public record New() : TicketStatus()
{
    public override bool CanTransitionTo(TicketStatus newStatus)
        => newStatus is Opened;
}

public record Opened() : TicketStatus()
{
    public override bool CanTransitionTo(TicketStatus newStatus)
        => newStatus is InProgress or Escalated or Closed;
}

public record InProgress() : TicketStatus()
{
    public override bool CanTransitionTo(TicketStatus newStatus)
        => newStatus is WaitingForUser or Resolved or Escalated;
}

public record WaitingForUser() : TicketStatus()
{
    public override bool CanTransitionTo(TicketStatus newStatus)
        => newStatus is InProgress or Resolved;
}

public record Escalated() : TicketStatus()
{
    public override bool CanTransitionTo(TicketStatus newStatus)
        => newStatus is InProgress or Resolved;
}

public record Resolved() : TicketStatus()
{
    public override bool CanTransitionTo(TicketStatus newStatus)
        => newStatus is Closed or Reopened;
}

public record Reopened() : TicketStatus()
{
    public override bool CanTransitionTo(TicketStatus newStatus)
        => newStatus is InProgress or Escalated;
}

public record Closed() : TicketStatus()
{
    public override bool CanTransitionTo(TicketStatus newStatus)
        => newStatus is Reopened;
}