using СС.Contracts.Shared;

namespace СС.Contracts.Appointments.Events;

public sealed record AppointmentCreatedEvent(Guid AppointmentId, Guid ClientId, Guid PractitionerId, DateTimeRange TimeSlot);