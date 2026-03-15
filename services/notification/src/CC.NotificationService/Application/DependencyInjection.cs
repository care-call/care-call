namespace CC.NotificationService.Application;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplication()
        {
            services.AddMediator(opts =>
            {
                opts.ServiceLifetime = ServiceLifetime.Scoped;
                opts.Assemblies = [typeof(DependencyInjection).Assembly];
            });
            services.AddSingleton(TimeProvider.System);
            return services;
        }
    }
}