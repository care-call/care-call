namespace CC.AppointmentService.Api.Contracts;

public sealed record CancelAppointmentRequest(Guid AppointmentId, Guid ClientId, string Reason);