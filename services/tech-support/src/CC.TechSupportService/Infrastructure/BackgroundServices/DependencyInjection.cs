using CC.TechSupportService.Domain.Constants;
using Npgsql;

namespace CC.TechSupportService.Infrastructure.BackgroundServices;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddBackgroundServices()
            => services.AddTicketRateLimitCleanup();
    }
}