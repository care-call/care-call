using CC.Shared.Domain;
using CC.Shared.Domain.TimeRanges;

namespace CC.AppointmentService.Domain.Appointments.Events;

public sealed record AppointmentCreatedDomainEvent(Guid AppointmentId, Guid ClientId, Guid PractitionerId,
    DateTimeRange TimeSlot) : IDomainEvent;