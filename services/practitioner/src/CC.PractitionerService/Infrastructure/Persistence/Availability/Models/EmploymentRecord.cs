using CC.Shared.Domain.TimeRanges;

namespace CC.PractitionerService.Infrastructure.Persistence.Availability.Models;

public sealed class EmploymentRecord
{
    public Guid Id { get; set; }
    public Guid PractitionerId { get; set; }
    public required string ExternalEmploymentKey { get; set; }
    public DateTimeRange Period { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}
