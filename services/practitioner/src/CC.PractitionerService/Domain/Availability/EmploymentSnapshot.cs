using CC.Shared.Domain.TimeRanges;

namespace CC.PractitionerService.Domain.Availability;

public sealed record EmploymentSnapshot(
    Guid PractitionerId,
    string ExternalEmploymentKey,
    DateTimeRange Period,
    DateTime CreatedAt);