using CC.Shared.Domain;
using СС.Contracts.Shared;

namespace СС.Contracts.Appointments.Events;

public sealed record AppointmentCreatedEvent(Guid AppointmentId, Guid PractitionerId, DateTimeRange TimeSlot) : IDomainEvent;