namespace CC.AppointmentService.Api.Contracts.Appointments;

public sealed record TransferAppointmentRequest(DateTime From, DateTime To, Guid ClientId);