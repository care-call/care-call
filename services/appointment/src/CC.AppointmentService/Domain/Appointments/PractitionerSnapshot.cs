using CC.Shared.Domain;

namespace CC.AppointmentService.Domain.Appointments;

public record PractitionerSnapshot
{
    public required FullName FullName { get; init; }
}