using CC.AppointmentService.Application.Dependencies.UnitOfWork;
using CC.AppointmentService.Domain.Appointments.Repositories;
using CC.AppointmentService.Domain.Feedback.Repositories;
using CC.AppointmentService.Infrastructure.Persistence.Appointments;
using CC.AppointmentService.Infrastructure.Persistence.Feedback;
using Microsoft.EntityFrameworkCore;

namespace CC.AppointmentService.Infrastructure.Persistence;

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
            return services
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IAppointmentsRepository, AppointmentsRepository>()
                .AddScoped<IReviewRepository, FeedbackRepository>();
        }
    }
}