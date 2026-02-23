using CC.PractitionerService.Domain.WorkSchedules;

namespace CC.PractitionerService.Api.Contracts.Common;

public sealed record AdjustmentDto
{
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public AdjustmentType AdjustmentType { get; init; }
}