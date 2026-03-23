using CC.AppointmentService.Application.Dependencies.BackgroundJobs;
using CC.AppointmentService.Infrastructure.BackgroundJobs.Jobs;
using Hangfire;
using Hangfire.PostgreSql;

namespace CC.AppointmentService.Infrastructure.BackgroundJobs;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddBackgroundJobs(IConfiguration configuration)
        {
            services.AddHangfire(hangfire =>
            {
                hangfire.UseSimpleAssemblyNameTypeSerializer();
                hangfire.UseRecommendedSerializerSettings();
                hangfire.UsePostgreSqlStorage(options =>
                {
                    options.UseNpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));
                });
            });

            services.AddHangfireServer();
            services.AddScoped<IAppointmentBackgroundTasks, AppointmentBackgroundTasks>();
            services.AddScoped<CallCreationJob>();
            return services;
        }
    }
}