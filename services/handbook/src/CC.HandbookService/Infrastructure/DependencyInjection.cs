using CC.HandbookService.Infrastructure.Persistence;
using CC.HandbookService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

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