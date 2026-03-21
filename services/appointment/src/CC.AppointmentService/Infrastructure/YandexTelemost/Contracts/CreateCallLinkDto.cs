using System.Text.Json.Serialization;

namespace CC.AppointmentService.Infrastructure.YandexTelemost.Contracts;


public sealed record CreateCallLinkDto
{
    [JsonPropertyName("waiting_room_level")]
    public string RoomLevel { get; set; }
    [JsonPropertyName("live_stream")]
    public LiveStreamDetails LiveStreamDetails { get; set; }
}