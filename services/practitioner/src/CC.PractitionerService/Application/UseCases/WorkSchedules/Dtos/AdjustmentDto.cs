using CC.PractitionerService.Domain.WorkSchedules;

namespace CC.PractitionerService.Application.UseCases.WorkSchedules.Dtos;

public sealed record AdjustmentDto
{
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }
    public required AdjustmentType AdjustmentType { get; init; }
}