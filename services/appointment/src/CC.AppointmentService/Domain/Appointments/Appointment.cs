using CC.Shared.Domain;
using CC.Shared.Domain.TimeRanges;

namespace CC.AppointmentService.Domain.Appointments;

public sealed class Appointment(Guid id) : AggregationRoot<Guid>(id)
{
    public required Guid ClientId { get; init; }
    public required Guid PractitionerId { get; init; }
    public required DateTimeRange TimeSlot { get; set; }
    public required AppointmentStatus Status { get; set; }
    public required ClientSnapshot ClientSnapshot { get; init; }
    public required PractitionerSnapshot PractitionerSnapshot { get; init; }
    public Uri? CallUrl { get; set; }
    public string? CancellationReason { get; private set; }
    public DateTime? EndedAt { get; set; }
    
    public void Cancel(string reason)
    {
        CancellationReason = reason;
        Status = AppointmentStatus.Cancelled;
    }
    
    public void Complete(DateTime endedAt)
    {
        Status = AppointmentStatus.Completed;
        EndedAt = endedAt;
    }
}