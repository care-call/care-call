using CC.NotificationService.Infrastructure.Persistence;

namespace CC.NotificationService.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(IConfiguration configuration)
            => services.AddPersistence(configuration);
    }
}