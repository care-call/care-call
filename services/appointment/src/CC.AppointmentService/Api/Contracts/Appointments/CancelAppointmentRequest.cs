namespace CC.AppointmentService.Api.Contracts.Appointments;

public sealed record CancelAppointmentRequest(Guid AppointmentId, Guid ClientId, string Reason);