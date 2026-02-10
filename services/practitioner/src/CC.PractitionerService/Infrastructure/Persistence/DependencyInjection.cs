using CC.PractitionerService.Application.Dependencies.UnitOfWork;
using CC.PractitionerService.Domain.Practitioners.Repositories;
using CC.PractitionerService.Domain.WorkSchedules.Repositories;
using CC.PractitionerService.Infrastructure.Persistence.Practitioners;
using CC.PractitionerService.Infrastructure.Persistence.Seeding;
using CC.PractitionerService.Infrastructure.Persistence.WorkSchedules;
using Microsoft.EntityFrameworkCore;

namespace CC.PractitionerService.Infrastructure.Persistence;

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
                    .UseSnakeCaseNamingConvention()
                    .UseAsyncSeeding(async (ctx, _, ct) => await new PractitionerProfileSeeder(ctx).SeedAsync(ct))
                    .UseSeeding((ctx, _) => new PractitionerProfileSeeder(ctx).SeedAsync(CancellationToken.None).GetAwaiter().GetResult()));
        }

        public IServiceCollection AddRepositories()
        {
            return services
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IPractitionerProfileRepository, PractitionerProfileRepository>()
                .AddScoped<IWorkScheduleRepository, WorkScheduleRepository>();
        }
    }
}