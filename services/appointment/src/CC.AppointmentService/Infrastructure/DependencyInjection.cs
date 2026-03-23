using CC.AppointmentService.Infrastructure.BackgroundJobs;
using CC.AppointmentService.Infrastructure.Persistence;
using CC.AppointmentService.Infrastructure.YandexTelemost;

namespace CC.AppointmentService.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(IConfiguration configuration)
        {
            return services
                .AddPersistence(configuration)
                .AddBackgroundJobs(configuration)
                .AddYandexTelemost(configuration);
        }
    }
}