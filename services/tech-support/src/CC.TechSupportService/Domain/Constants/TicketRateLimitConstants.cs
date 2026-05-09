namespace CC.TechSupportService.Domain.Constants;

public static class TicketRateLimitConstants
{
    public const int MaxTicketsInWindow = 5;
    public static readonly TimeSpan Window = TimeSpan.FromHours(1);
}