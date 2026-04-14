using CC.AppointmentService.Application.Dependencies.UnitOfWork;
using CC.AppointmentService.Application.Dependencies;
using CC.AppointmentService.Application.Dependencies.AppointmentsQuery;
using CC.AppointmentService.Domain.Appointments.Repositories;
using CC.AppointmentService.Domain.Feedbacks.Repositories;
using CC.AppointmentService.Infrastructure.Persistence.Appointments;
using CC.AppointmentService.Infrastructure.Persistence.Feedbacks;
using CC.AppointmentService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace CC.AppointmentService.Infrastructure.Persistence;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddPersistence(IConfiguration configuration)
        {
            return services.AddEfCore(configuration).AddRepositories().AddServices();
        }

        private IServiceCollection AddEfCore(IConfiguration configuration)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            return services.AddNpgsql<DatabaseContext>(
                configuration.GetConnectionString("DefaultConnection"),
                _ => { },
                dbCtxBuilder => dbCtxBuilder
                    .UseSnakeCaseNamingConvention());
        }

        public IServiceCollection AddServices()
        {
            return services.AddScoped<IAppointmentQueryService, AppointmentQueryService>();
        }

        private IServiceCollection AddRepositories()
        {
            return services
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IPractitionerAppointmentsQuery, PractitionerAppointmentsQuery>()
                .AddScoped<IAppointmentsRepository, AppointmentsRepository>()
                .AddScoped<IFeedbackRepository, FeedbackRepository>();
        }
    }
}