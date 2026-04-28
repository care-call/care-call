using CC.HandbookService.Infrastructure.Persistence;
using CC.HandbookService.Infrastructure.Services;

namespace CC.HandbookService.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(IConfiguration configuration)
        {
            return services
                .AddPersistence(configuration)
                .AddServices();
        }
    }
}