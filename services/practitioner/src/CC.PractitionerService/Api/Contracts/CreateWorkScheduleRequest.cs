using CC.PractitionerService.Api.Contracts.Common;

namespace CC.PractitionerService.Api.Contracts;

public sealed record CreateWorkScheduleRequest
{
    public required string TimeZoneId { get; init; }
    public required IReadOnlyList<WeeklyRecurrenceDto> Recurrences { get; init; } = [];
    public required WorkScheduleValidityPeriodDto ValidityPeriod { get; init; }
    public required TimeSpan SessionDuration { get; init; }
}