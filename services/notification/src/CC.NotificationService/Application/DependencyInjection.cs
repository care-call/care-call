using CC.NotificationService.Application.Dependencies;
using CC.NotificationService.Application.Services;

namespace CC.NotificationService.Application;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplication()
        {
            services.AddSingleton(TimeProvider.System);
            services.AddScoped<ITemplateService, TemplateService>();
            return services;          
        }
    }
}