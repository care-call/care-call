using CC.TechSupportService.Domain.ValueObjects.Ticket;
using CC.TechSupportService.Features.Tickets.Enums;

namespace CC.TechSupportService.Features.Tickets.Extensions;

public static class TicketStatusExtensions
{
    extension(TicketStatusType status)
    {
        public TicketStatus ToTicketStatus() =>
            status switch
            {
                TicketStatusType.New => new New(),
                TicketStatusType.Opened => new Opened(),
                TicketStatusType.InProgress => new InProgress(),
                TicketStatusType.WaitingForUser => new WaitingForUser(),
                TicketStatusType.Escalated => new Escalated(),
                TicketStatusType.Resolved => new Resolved(),
                TicketStatusType.Reopened => new Reopened(),
                TicketStatusType.Closed => new Closed(),
                _ => throw new ArgumentOutOfRangeException(nameof(status), $@"Некорректное значение статуса: {status}")
            };
    }
}