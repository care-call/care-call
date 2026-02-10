using CC.PractitionerService.Infrastructure.Persistence;

namespace CC.PractitionerService.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(IConfiguration configuration)
        {
            return services.AddPersistence(configuration);
        }
    }
}