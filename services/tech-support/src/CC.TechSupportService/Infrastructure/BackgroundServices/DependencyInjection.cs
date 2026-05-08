using CC.TechSupportService.Infrastructure.BackgroundServices.CleanUpServices;

namespace CC.TechSupportService.Infrastructure.BackgroundServices;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddBackgroundServices()
            => services.AddCleanupServices();
    }
}