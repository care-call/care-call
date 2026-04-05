namespace CC.HandbookService.Application;

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
            
            return services;
        }
    }
}