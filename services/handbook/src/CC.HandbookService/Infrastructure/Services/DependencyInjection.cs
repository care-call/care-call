using CC.HandbookService.Application.Dependencies.Handbook;
using CC.HandbookService.Infrastructure.Services.Handbook;

namespace CC.HandbookService.Infrastructure.Services;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddServices()
        {
            return services
                .AddScoped<IHandbookRegistry, HandbookRegistry>()
                .AddScoped<IHandbookLoader, HandbookLoader>();
        }
    }
}