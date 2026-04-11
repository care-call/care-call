namespace CC.NotificationService.Application;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplication()
        {
            services.AddSingleton(TimeProvider.System);
            return services;
        }
    }
}