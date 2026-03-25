using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CC.Common.Logging;

public static class LoggingExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddLogging(
            IConfiguration configuration
        )
        {
            services.AddSerilog((serviceProvider, loggerConfiguration) =>
            {
                loggerConfiguration
                    .ReadFrom.Configuration(configuration)
                    .ReadFrom.Services(serviceProvider)
                    .Enrich.FromLogContext()
                    .Enrich.With();
            });
            return services;
        }
    }
    
}