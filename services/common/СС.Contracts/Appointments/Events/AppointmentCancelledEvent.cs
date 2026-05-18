namespace СС.Contracts.Appointments.Events;

public sealed record AppointmentCancelledEvent(Guid AppointmentId, Guid ClientId, Guid PractitionerId, string Reason);