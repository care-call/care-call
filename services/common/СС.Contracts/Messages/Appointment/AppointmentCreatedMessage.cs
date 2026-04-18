using СС.Contracts.Shared;

namespace СС.Contracts.Messages.Appointment;

public sealed record AppointmentCreatedMessage(Guid AppointmentId, Guid PractitionerId, DateTimeRange TimeSlot);