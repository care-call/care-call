using CC.AppointmentService.Domain.Appointments.Events;
using ContractDateTimeRange = СС.Contracts.Shared.DateTimeRange;
using СС.Contracts.Appointments.Events;

namespace CC.AppointmentService.Infrastructure.Messaging;

public static class AppointmentIntegrationEventHandler
{
    public static AppointmentCreatedEvent Handle(AppointmentCreatedDomainEvent domainEvent) =>
        new(
            domainEvent.AppointmentId,
            domainEvent.ClientId,
            domainEvent.PractitionerId,
            new ContractDateTimeRange(domainEvent.TimeSlot.From, domainEvent.TimeSlot.To));

    public static AppointmentTransferredEvent Handle(AppointmentTransferredDomainEvent domainEvent) =>
        new(
            domainEvent.AppointmentId,
            domainEvent.ClientId,
            domainEvent.PractitionerId,
            new ContractDateTimeRange(domainEvent.TimeSlot.From, domainEvent.TimeSlot.To));

    public static AppointmentCancelledEvent Handle(AppointmentCancelledDomainEvent domainEvent) =>
        new(
            domainEvent.AppointmentId,
            domainEvent.ClientId,
            domainEvent.PractitionerId,
            domainEvent.Reason);
}
