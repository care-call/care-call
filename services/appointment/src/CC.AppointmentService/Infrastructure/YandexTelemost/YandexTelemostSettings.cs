namespace CC.AppointmentService.Infrastructure.YandexTelemost;

public sealed record YandexTelemostSettings
{
    public string Token { get; init; }
    public string BaseUrl { get; init; }
}