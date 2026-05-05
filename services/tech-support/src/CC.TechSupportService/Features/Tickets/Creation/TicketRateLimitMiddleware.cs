using StackExchange.Redis;
using Wolverine;

namespace CC.TechSupportService.Features.Tickets.Creation;

public class TicketRateLimitMiddleware(IConnectionMultiplexer connMultiplexer)
{
    public async Task<HandlerContinuation> BeforeAsync(CreateTicketRequest command, HttpResponse response)
    {
        var db = connMultiplexer.GetDatabase();
        var key = $"tickets:{command.ReporterId}";
    
        var count = await db.StringIncrementAsync(key);
    
        if (count > 5)
        {
            await db.StringDecrementAsync(key);
            
            response.StatusCode = 429;
            await response.WriteAsJsonAsync(new { error = "Лимит создания тикетов превышен. Максимум 5 в час" });
            return HandlerContinuation.Stop;
        }

        if (count == 1)
            await db.KeyExpireAsync(key, TimeSpan.FromHours(1));
        
        return HandlerContinuation.Continue;
    }
}