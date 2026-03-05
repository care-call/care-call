using CC.PractitionerService.Api.Contracts.Common;

namespace CC.PractitionerService.Api.Contracts;

public sealed record SaveAdjustmentsForScheduleRequest
{
    public required ICollection<AdjustmentDto> NewAdjustments { get; init; }
    public required ICollection<Guid> RemovedAdjustments { get; init; }
    public required Guid WorkScheduleId { get; init; }
    public required DateTime StartDate { get; init; }
};