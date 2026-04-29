using Microsoft.EntityFrameworkCore;

namespace CC.TechSupportService.Infrastructure.Persistence;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddPersistence(IConfiguration configuration)
        {
            services.AddNpgsql<DatabaseContext>(
                configuration.GetConnectionString("DefaultConnection"),
                _ => { },
                dbCtxBuilder =>
                {
                    dbCtxBuilder.UseSnakeCaseNamingConvention();
                });

            return services;
        }
    }
}