using CC.NotificationService.Application.Dependencies;
using CC.NotificationService.Application.Dependencies.UnitOfWork;
using CC.NotificationService.Domain;
using CC.NotificationService.Domain.Interfaces;
using CC.NotificationService.Infrastructure.Persistence.Notifications;
using CC.NotificationService.Infrastructure.Repositories;
using CC.NotificationService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace CC.NotificationService.Infrastructure.Persistence;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddPersistence(IConfiguration configuration)       
          => services
            .AddEfCore(configuration)
            .AddDependencies();
        
        public IServiceCollection AddEfCore(IConfiguration configuration)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            return services.AddNpgsql<DatabaseContext>(
                configuration.GetConnectionString("DefaultConnection"),
                _ => { },
                dbCtxBuilder => dbCtxBuilder
                    .UseSnakeCaseNamingConvention());
        }

        public IServiceCollection AddDependencies()        
           => services
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<INotificationsRepository, NotificationsRepository>()
                .AddScoped<ITemplateRepository, TemplateRepository>()
                .AddScoped<ITemplateVersionRepository, TemplateVersionRepository>()
                .AddScoped<ITemplateVersionQuery, TemplateVersionQueryService>()
                .AddScoped<ITemplateQuery, TemplateQueryService>();       
    }
}