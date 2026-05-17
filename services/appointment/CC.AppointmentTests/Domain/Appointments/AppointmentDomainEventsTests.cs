using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Appointments.Events;
using CC.Shared.Domain;
using CC.Shared.Domain.TimeRanges;
using Shouldly;

namespace CC.AppointmentTests.Domain.Appointments;

public class AppointmentDomainEventsTests
{
    private readonly DateTime _now = new(2026, 04, 06, 10, 0, 0, DateTimeKind.Utc);
    
    private Appointment CreateAppointment() =>
        Appointment.Create(Guid.NewGuid(), Guid.NewGuid(), Slot(1),
            new ClientSnapshot { FullName = new FullName("Тест‚", "Клиент", null) },
            new PractitionerSnapshot { FullName = new FullName("Тест‚", "Врач", null) }, _now).Value;

    private Appointment BuildAppointment() =>
        AppointmentBuilder.Create(Guid.NewGuid(), Guid.NewGuid()).WithTimeSlot(Slot(1)).Build();

    private DateTimeRange Slot(int days) => new(_now.AddDays(days), _now.AddDays(days).AddHours(1));
    
    [Fact]
    public void Creating_appointment_adds_domain_event()
    {
        var appointment = CreateAppointment();

        appointment.DomainEvents.ShouldHaveSingleItem()
            .ShouldBeOfType<AppointmentCreatedDomainEvent>();
    }

    [Fact]
    public void Transferring_appointment_adds_domain_event()
    {
        var appointment = BuildAppointment();
        appointment.Transfer(Slot(2), _now);

        appointment.DomainEvents.ShouldHaveSingleItem()
            .ShouldBeOfType<AppointmentTransferredDomainEvent>();
    }

    [Fact]
    public void Cancelling_appointment_adds_domain_event()
    {
        var appointment = BuildAppointment();
        appointment.Cancel(CancellationReason.From("Клиент попросил отменить запись"), _now);

        appointment.DomainEvents.ShouldHaveSingleItem()
            .ShouldBeOfType<AppointmentCancelledDomainEvent>();
    }
}