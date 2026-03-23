using CC.AppointmentService.Application.Dependencies.YandexTelemost;
using Microsoft.Extensions.Options;

namespace CC.AppointmentService.Infrastructure.YandexTelemost;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddYandexTelemost(IConfiguration configuration)
        {
            services.AddOptions<YandexTelemostSettings>()
                .BindConfiguration("YandexTelemostSettings")
                .ValidateDataAnnotations()
                .ValidateOnStart();
            
            services.AddHttpClient<IYandexTelemostService, YandexTelemostService>((serviceProvider, client) =>
                {
                    var options = serviceProvider.GetRequiredService<IOptions<YandexTelemostSettings>>().Value;

                    client.DefaultRequestHeaders.Add("Authorization", options.Token);
                    client.BaseAddress = new Uri(options.BaseUrl);
                })
                .AddStandardResilienceHandler(options =>
                {
                    options.Retry.MaxRetryAttempts = 2;

                    options.Retry.DelayGenerator = context =>
                    {
                        var delay = TimeSpan.FromSeconds(Math.Pow(2, context.AttemptNumber));
                        return new ValueTask<TimeSpan?>(delay);
                    };
                });

            return services;
        }
    }
}