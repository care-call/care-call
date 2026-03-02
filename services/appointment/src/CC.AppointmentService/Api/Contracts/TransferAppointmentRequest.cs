namespace CC.AppointmentService.Api.Contracts;

public sealed record TransferAppointmentRequest(DateTime From, DateTime To, Guid ClientId);