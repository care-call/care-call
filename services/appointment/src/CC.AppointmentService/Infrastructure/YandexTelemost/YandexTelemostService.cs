using System.Net.Http.Headers;
using CC.AppointmentService.Application.Dependencies.YandexTelemost;
using CC.AppointmentService.Infrastructure.YandexTelemost.Contracts;

namespace CC.AppointmentService.Infrastructure.YandexTelemost;

public sealed class YandexTelemostService(
    HttpClient client,
    ILogger<YandexTelemostService> logger) : IYandexTelemostService
{
    public async Task<CreateCallLingResponse?> CreateCallLinkAsync(CancellationToken ct)
    {
        var content = new StringContent("""
                                        {
                                          "waiting_room_level": "PUBLIC",
                                          "live_stream": {
                                            "access_level": "PUBLIC",
                                            "title": "Example conference created via API",
                                            "description": "Some description of example conference created via API"
                                          }
                                        }
                                        """);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var response = await client.PostAsync("conferences", content, ct);
        
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Ошибка статус код: " +
                            "{statusCode}, {responseContent}", response.StatusCode,
                await response.Content.ReadAsStringAsync(ct));
            return null;
        }
        
        var result = await response.Content.ReadFromJsonAsync<CreateCallLinkResult>(ct);
        return new CreateCallLingResponse(result!.JoinUrl);
    } 
}