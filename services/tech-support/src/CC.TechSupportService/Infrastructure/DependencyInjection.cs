using CC.TechSupportService.Infrastructure.BackgroundServices;
using CC.TechSupportService.Infrastructure.Persistence;

namespace CC.TechSupportService.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(IConfiguration configuration)
            => services
                .AddPersistence(configuration)
                .AddBackgroundServices();
    }
}