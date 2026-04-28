using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.OpenTelemetry;

namespace CC.Common.Logging;

public static class LoggingExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddLogger(IConfiguration configuration)
        {
            services.AddSerilog((serviceProvider, loggerConfiguration) =>
            {
                var hostEnvironment = serviceProvider.GetRequiredService<IHostEnvironment>();

                loggerConfiguration
                    .ReadFrom.Configuration(configuration)
                    .ReadFrom.Services(serviceProvider)
                    .Enrich.FromLogContext();

                var otlpLogsEndpoint = GetOtlpLogsEndpoint(configuration);
                if (!string.IsNullOrWhiteSpace(otlpLogsEndpoint))
                {
                    loggerConfiguration.WriteTo.OpenTelemetry(options =>
                    {
                        options.Endpoint = otlpLogsEndpoint;
                        options.Protocol = GetOtlpProtocol(configuration);
                        options.ResourceAttributes = new Dictionary<string, object>
                        {
                            ["service.name"] = hostEnvironment.ApplicationName,
                            ["deployment.environment"] = hostEnvironment.EnvironmentName,
                        };
                    });
                }
            });
            return services;
        }

        private static string? GetOtlpLogsEndpoint(IConfiguration configuration)
        {
            return configuration["OTEL_EXPORTER_OTLP_LOGS_ENDPOINT"]
                ?? configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
        }

        private static OtlpProtocol GetOtlpProtocol(IConfiguration configuration)
        {
            var protocol = configuration["OTEL_EXPORTER_OTLP_LOGS_PROTOCOL"]
                ?? configuration["OTEL_EXPORTER_OTLP_PROTOCOL"];

            return protocol?.ToLowerInvariant() switch
            {
                "http/protobuf" => OtlpProtocol.HttpProtobuf,
                _ => OtlpProtocol.Grpc
            };
        }
    }
}
