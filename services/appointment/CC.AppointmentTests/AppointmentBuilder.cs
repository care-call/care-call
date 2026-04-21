using CC.AppointmentService.Domain.Appointments;
using CC.Shared.Domain;
using CC.Shared.Domain.TimeRanges;

namespace CC.AppointmentTests;

/// <summary>
/// Строитель записи для тестов, чтобы не дублировать сложное создание.
/// </summary>
public sealed class AppointmentBuilder
{
    private readonly Guid _id;
    private readonly Guid _clientId;
    private AppointmentStatus _status = AppointmentStatus.Planned;
    private DateTimeRange? _timeSlot;

    private AppointmentBuilder(Guid id, Guid clientId)
    {
        _id = id;
        _clientId = clientId;
        _timeSlot = null;
    }

    public static AppointmentBuilder Create(Guid id, Guid clientId) => new(id, clientId);

    public AppointmentBuilder WithStatus(AppointmentStatus status)
    {
        _status = status;
        return this;
    }
   
    public AppointmentBuilder WithTimeSlot(DateTimeRange timeSlot)
    {
        _timeSlot = timeSlot;
        return this;
    }

    public Appointment Build()
    {
        var finalSlot = _timeSlot ?? new DateTimeRange(
            new DateTime(2026, 03, 29, 8, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 03, 29, 9, 0, 0, DateTimeKind.Utc));

        return new Appointment(_id)
        {
            ClientId = _clientId,
            PractitionerId = Guid.NewGuid(),
            TimeSlot = finalSlot,
            Status = _status,
            ClientSnapshot = new ClientSnapshot { FullName = new FullName("Test", "Client", null) },
            PractitionerSnapshot = new PractitionerSnapshot { FullName = new FullName("Test", "Doctor", null) },
        };
    }
}