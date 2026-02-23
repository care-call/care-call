using CC.AppointmentService.Infrastructure.Persistence;

namespace CC.AppointmentService.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(IConfiguration configuration)
        {
            return services.AddPersistence(configuration);
        }
    }
}