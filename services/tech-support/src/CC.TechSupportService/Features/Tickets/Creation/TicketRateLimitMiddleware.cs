using System.Runtime.ExceptionServices;
using CC.TechSupportService.Domain.Constants;
using CC.TechSupportService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Wolverine;

namespace CC.TechSupportService.Features.Tickets.Creation;

public class TicketRateLimitMiddleware()
{
    private bool _incremented;

    public async Task<HandlerContinuation> BeforeAsync(
        CreateTicketRequest command,
        NpgsqlDataSource dataSource,
        HttpResponse response)
    {
        await using var conn = await dataSource.OpenConnectionAsync();
        
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = """
                          INSERT INTO ticket_rate_limits (reporter_id, count, created_at)
                          VALUES ($1, 1, $2)
                          ON CONFLICT (reporter_id)
                          DO UPDATE SET 
                              count = CASE 
                                  WHEN now() - ticket_rate_limits.created_at >= $3 
                                  THEN 1
                                  ELSE ticket_rate_limits.count + 1
                              END,
                              created_at = CASE 
                                  WHEN now() - ticket_rate_limits.created_at >= $3 
                                  THEN $2
                                  ELSE ticket_rate_limits.created_at
                              END
                          RETURNING count
                          """;

        cmd.Parameters.AddWithValue(command.ReporterId);
        cmd.Parameters.AddWithValue(DateTime.UtcNow);
        cmd.Parameters.AddWithValue(TicketRateLimitConstants.Window);

        var cmdResult = (await cmd.ExecuteScalarAsync())?.ToString();
        if (!short.TryParse(cmdResult, out var count))
            throw new InvalidOperationException("Неверный тип данных в строке");
        
        if (count > TicketRateLimitConstants.MaxTicketsInWindow)
        {
            response.StatusCode = 429;
            await response.WriteAsJsonAsync(new { error = "Превышен лимит создания тикетов. Максимум 5 тикетов в час" });
            return HandlerContinuation.Stop;
        }

        _incremented = true;
        
        return HandlerContinuation.Continue;
    }
    
    public async Task OnException(
        Exception ex,
        CreateTicketRequest command,
        NpgsqlDataSource dataSource)
    {
        if (!_incremented)
            return;

        await using var conn = await dataSource.OpenConnectionAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = """
                          UPDATE ticket_rate_limits 
                          SET count = count - 1 
                          WHERE reporter_id = $1 AND count > 0
                          """;
        cmd.Parameters.AddWithValue(command.ReporterId);
        await cmd.ExecuteNonQueryAsync();
        
        ExceptionDispatchInfo.Throw(ex);
    }
}