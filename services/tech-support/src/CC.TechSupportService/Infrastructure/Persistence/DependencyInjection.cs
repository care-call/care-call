using CC.TechSupportService.Domain.Abstractions;
using CC.TechSupportService.Infrastructure.Persistence.Repositories;
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
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}