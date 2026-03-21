using System.Net.Http.Headers;
using CC.AppointmentService.Application.Dependencies.YandexTelemost;
using CC.AppointmentService.Infrastructure.YandexTelemost.Contracts;

namespace CC.AppointmentService.Infrastructure.YandexTelemost;

public sealed class YandexTelemostService(HttpClient client) : IYandexTelemostService
{
    public async Task<CreateCallLingResponse> CreateCallLinkAsync()
    {
        // var responce = await client.PostAsJsonAsync("conferences", new CreateCallLinkDto()
        // {
        //     RoomLevel = "PUBLIC",
        //     LiveStreamDetails = new LiveStreamDetails()
        //     {
        //         AccessLevel = "PUBLIC",
        //         Title = "Тест звонка",
        //         Description = "Тестируем звонок в телемосте"
        //     }
        // } );
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
        var responce = await client.PostAsync("conferences", content);

        var result = await responce.Content.ReadFromJsonAsync<CreateCallLinkResult>();
        return new CreateCallLingResponse(result!.JoinUrl);
    } 
}