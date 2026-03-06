using CC.PractitionerService.Domain.WorkSchedules;

namespace CC.PractitionerService.Api.Contracts.Common;

public sealed record CreateAdjustmentDto
{
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }
    public required AdjustmentType AdjustmentType { get; init; }
}