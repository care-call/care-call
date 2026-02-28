using CC.PractitionerService.Api.Contracts.Common;

namespace CC.PractitionerService.Api.Contracts;

public sealed record AddAdjustmentsForScheduleRequest
{
    public required ICollection<AdjustmentDto>  Adjustments { get; init; }
};