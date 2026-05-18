using СС.Contracts.Shared;

namespace СС.Contracts.Appointments.Events;

public sealed record AppointmentTransferredEvent(Guid AppointmentId, Guid ClientId, Guid PractitionerId, DateTimeRange TimeSlot);