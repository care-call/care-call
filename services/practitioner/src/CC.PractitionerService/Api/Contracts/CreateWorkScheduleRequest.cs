using CC.PractitionerService.Api.Contracts.Common;
using CC.PractitionerService.Infrastructure.OpenApi;
using Microsoft.OpenApi;

namespace CC.PractitionerService.Api.Contracts;

public sealed record CreateWorkScheduleRequest
{
    [OpenApiExampleAttribute("Europe/Moscow")]
    public required string TimeZoneId { get; init; }
    public required IReadOnlyList<WeeklyRecurrenceDto> Recurrences { get; init; } = [];
    public required WorkScheduleValidityPeriodDto ValidityPeriod { get; init; }
    public required TimeSpan SessionDuration { get; init; }
}