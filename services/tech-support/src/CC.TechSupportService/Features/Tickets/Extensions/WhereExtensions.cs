using CC.TechSupportService.Domain.Entities;
using CC.TechSupportService.Domain.ValueObjects.Ticket;
using CC.TechSupportService.Features.Tickets.Enums;

namespace CC.TechSupportService.Features.Tickets.Extensions;

public static class WhereExtensions
{
    extension(IQueryable<Ticket> query)
    {
        public IQueryable<Ticket> WhereStatus(string? status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return query;

            var parsedStatus = ParseStatus(status);

            return parsedStatus is null ? query.Where(t => false) : query.Where(t => t.Status == parsedStatus);
        }
        private static TicketStatus? ParseStatus(string status)
        {
            if (!Enum.TryParse<TicketStatusType>(status, ignoreCase: true, out var statusType)
                || !Enum.IsDefined(statusType))
                throw new ArgumentOutOfRangeException(nameof(status), $"Некорректное значение статуса: {status}");
                
            return Enum.Parse<TicketStatusType>(status) switch
            {
                TicketStatusType.New => new New(),
                TicketStatusType.Opened => new Opened(),
                TicketStatusType.InProgress => new InProgress(),
                TicketStatusType.WaitingForUser => new WaitingForUser(),
                TicketStatusType.Escalated => new Escalated(),
                TicketStatusType.Resolved => new Resolved(),
                TicketStatusType.Reopened => new Reopened(),
                TicketStatusType.Closed => new Closed(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}