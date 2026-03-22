using System.ComponentModel.DataAnnotations;

namespace CC.AppointmentService.Infrastructure.YandexTelemost;

public sealed record YandexTelemostSettings
{
    [Required]
    public required string Token { get; init; }
    [Required]
    public required string BaseUrl { get; init; }
}