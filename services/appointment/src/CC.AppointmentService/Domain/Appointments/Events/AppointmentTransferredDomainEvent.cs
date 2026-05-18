using CC.Shared.Domain;
using CC.Shared.Domain.TimeRanges;

namespace CC.AppointmentService.Domain.Appointments.Events;

public sealed record AppointmentTransferredDomainEvent(Guid AppointmentId, Guid ClientId, Guid PractitionerId,
    DateTimeRange TimeSlot) : IDomainEvent;