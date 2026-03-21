using System.Text.Json.Serialization;

namespace CC.AppointmentService.Infrastructure.YandexTelemost.Contracts;

public class LiveStreamDetails
{
    [JsonPropertyName("access_level")]
    public string AccessLevel { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}