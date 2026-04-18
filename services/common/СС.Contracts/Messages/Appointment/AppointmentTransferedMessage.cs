using СС.Contracts.Shared;

namespace СС.Contracts.Messages.Appointment;

public sealed record AppointmentTransferedMessage(Guid AppointmentId, DateTimeRange TimeSlot);
