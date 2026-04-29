using CC.TechSupportService.Domain.Enums;
using JasperFx.Core;

namespace CC.TechSupportService.Domain.ValueObjects.Ticket;

public abstract record TicketCategory(Priority Priority, TimeSpan FirstResponseDeadlineInHours);

public sealed record Bug() : TicketCategory(Priority.Low, 48.Hours());

public sealed record Question() : TicketCategory(Priority.Normal, 24.Hours());

public sealed record FeatureRequest() : TicketCategory(Priority.Low, 48.Hours());

public sealed record Complaint() : TicketCategory(Priority.High, 8.Hours());

public sealed record AccountIssue() : TicketCategory(Priority.Critical, 4.Hours());