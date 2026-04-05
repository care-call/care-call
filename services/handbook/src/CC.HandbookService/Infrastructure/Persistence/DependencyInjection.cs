using CC.HandbookService.Application;
using Microsoft.EntityFrameworkCore;

namespace CC.HandbookService.Infrastructure.Persistence;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddPersistence(IConfiguration configuration)
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
            return services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}