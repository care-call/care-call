using System.Text.Json.Serialization;

namespace CC.AppointmentService.Infrastructure.YandexTelemost.Contracts;

public record CreateCallLinkResult
{ 
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("join_url")]
    public string JoinUrl { get; set; }
    [JsonPropertyName("live_stream")]
    public  LiveStreamDetailsResponce LiveStreamDetails { get; set; }
}

public record LiveStreamDetailsResponce
{
    [JsonPropertyName("watch_url")]
    public string AccessLevel { get; set; }
}