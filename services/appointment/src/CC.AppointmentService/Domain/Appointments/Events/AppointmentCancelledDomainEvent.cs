using CC.Shared.Domain;

namespace CC.AppointmentService.Domain.Appointments.Events;

public sealed record AppointmentCancelledDomainEvent(Guid AppointmentId, Guid ClientId, Guid PractitionerId,
    string Reason) : IDomainEvent;