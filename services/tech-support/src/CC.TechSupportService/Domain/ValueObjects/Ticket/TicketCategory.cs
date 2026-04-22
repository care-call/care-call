using CC.TechSupportService.Domain.Enums;

namespace CC.TechSupportService.Domain.ValueObjects.Ticket;

public abstract record TicketCategory(string Name, Priority Priority)
{
    public abstract int FirstResponseDeadlineInHours { get; }
}

public sealed record Bug() : TicketCategory(nameof(Bug), Priority.Low)
{
    public override int FirstResponseDeadlineInHours => 48;
}

public sealed record Question() : TicketCategory(nameof(Question), Priority.Normal)
{
    public override int FirstResponseDeadlineInHours => 24;
}

public sealed record FeatureRequest() : TicketCategory(nameof(FeatureRequest), Priority.Low)
{
    public override int FirstResponseDeadlineInHours => 48;
}

public sealed record Complaint() : TicketCategory(nameof(Complaint), Priority.High)
{
    public override int FirstResponseDeadlineInHours => 8;
}

public sealed record AccountIssue() : TicketCategory(nameof(AccountIssue), Priority.Critical)
{
    public override int FirstResponseDeadlineInHours => 4;
}