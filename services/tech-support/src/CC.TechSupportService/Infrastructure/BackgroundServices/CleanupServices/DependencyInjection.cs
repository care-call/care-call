using CC.TechSupportService.Domain.Constants;
using Npgsql;

namespace CC.TechSupportService.Infrastructure.BackgroundServices.CleanUpServices;

public static class CleanupServices
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddCleanupServices()
        {
            services.AddTicketRateLimitCleanup();
            
            return services;
        }

        private IServiceCollection AddTicketRateLimitCleanup()
        {
            services.AddHostedService(sp => new TableCleanupService(
                sp.GetRequiredService<NpgsqlDataSource>(),
                "DELETE FROM ticket_rate_limits WHERE now() - created_at >= @window",
                [new NpgsqlParameter("window", TicketRateLimitConstants.Window)],
                TimeSpan.FromHours(1)));
        
            return services;
        }
    }
}