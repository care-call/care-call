using System.Text.Json.Serialization;

namespace CC.AppointmentService.Infrastructure.YandexTelemost.Contracts;

public record CreateCallLinkResult
{
    [JsonPropertyName("id")]
    [JsonRequired]
    public required string Id { get; init; }
    [JsonRequired]
    [JsonPropertyName("join_url")]
    public required string JoinUrl { get; init; }
    [JsonRequired]
    [JsonPropertyName("live_stream")]
    public required LiveStreamDetailsResponce LiveStreamDetails { get; init; }
}

public record LiveStreamDetailsResponce
{
    [JsonPropertyName("watch_url")]
    [JsonRequired]
    public required string AccessLevel { get; init; }
}