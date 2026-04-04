using CC.HandbookService.Application;
using CC.HandbookService.Application.Handbook;
using CC.HandbookService.Infrastructure.Handbook;
using Microsoft.EntityFrameworkCore;

namespace CC.HandbookService.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(IConfiguration configuration)
        {
            return services.AddEfCore(configuration).AddRepositories();
        }

        public IServiceCollection AddEfCore(IConfiguration configuration)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            return services.AddNpgsql<DatabaseContext>(
                configuration.GetConnectionString("DefaultConnection"),
                _ => { },
                dbCtxBuilder => dbCtxBuilder
                    .UseSnakeCaseNamingConvention());
        }

        public IServiceCollection AddRepositories()
        {
            return services
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IHandbookRegistry, HandbookRegistry>()
                .AddScoped<IHandbookLoader, HandbookLoader>();
        }
    }
}