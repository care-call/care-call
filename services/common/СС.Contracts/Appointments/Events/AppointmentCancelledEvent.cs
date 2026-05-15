using CC.Shared.Domain;

namespace СС.Contracts.Appointments.Events;

public sealed record AppointmentCancelledEvent(Guid AppointmentId, Guid ClientId, string Reason) : IDomainEvent;