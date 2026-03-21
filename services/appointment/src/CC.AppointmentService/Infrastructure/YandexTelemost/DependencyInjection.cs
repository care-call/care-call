using CC.AppointmentService.Application.Dependencies.YandexTelemost;
using Microsoft.Extensions.Options;

namespace CC.AppointmentService.Infrastructure.YandexTelemost;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddYandexTelemost(IConfiguration configuration)
        {
            services.Configure<YandexTelemostSettings>(configuration.GetSection("YandexTelemostSettings"));

            services.AddHttpClient<IYandexTelemostService, YandexTelemostService>((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<YandexTelemostSettings>>().Value;
  
                client.DefaultRequestHeaders.Add("Authorization", options.Token);
                client.BaseAddress = new Uri(options.BaseUrl);
            });

            return services;
        }
    }
}