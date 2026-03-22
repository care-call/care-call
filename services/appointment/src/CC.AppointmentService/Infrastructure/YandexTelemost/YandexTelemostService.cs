using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using CC.AppointmentService.Application.Dependencies.YandexTelemost;

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
                                            "title": "Онлайн запись на платформе Care-Call",
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

        var node = await JsonNode.ParseAsync(await response.Content.ReadAsStreamAsync(ct), cancellationToken: ct);
        var joinUrl = node?["join_link"]?.GetValue<string>();
        if (string.IsNullOrEmpty(joinUrl))
        {
            logger.LogError("Ошибка отсутствует ссылка на присоединению к звонку {responseContent}",
                await response.Content.ReadAsStringAsync(ct));
            return null; 
        }
        
        return new CreateCallLingResponse(joinUrl);
    } 
}