namespace CC.PractitionerService.Api.Contracts.Common;

public sealed record WorkScheduleValidityPeriodDto
{
    public required DateOnly From { get; init; }
    public DateOnly? To { get; init; }
}