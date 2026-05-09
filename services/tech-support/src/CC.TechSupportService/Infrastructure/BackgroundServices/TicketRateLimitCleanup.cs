using CC.TechSupportService.Domain.Constants;
using CC.TechSupportService.Infrastructure.BackgroundServices.SqlExecutor;
using Npgsql;

namespace CC.TechSupportService.Infrastructure.BackgroundServices;

public static class TicketRateLimitCleanup
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddTicketRateLimitCleanup()
        {
            services.AddHostedService(sp => new IntervalSqlExecutor(
                sp.GetRequiredService<NpgsqlDataSource>(),
                "DELETE FROM ticket_rate_limits WHERE now() - created_at >= @window",
                [new NpgsqlParameter("window", TicketRateLimitConstants.Window)],
                TimeSpan.FromHours(1),
                sp.GetRequiredService<ILoggerFactory>().CreateLogger("TicketRateLimitCleanup")));

            return services;
        }
    }
}