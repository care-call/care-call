using CC.Shared.Domain;

namespace CC.AppointmentService.Domain.Appointments;

public record ClientSnapshot
{
    public required FullName FullName { get; init; }
}