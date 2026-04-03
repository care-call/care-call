using CC.AppointmentService.Application.Dependencies.UnitOfWork;
using CC.AppointmentService.Application.UseCases.Feedbacks.Creation;
using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Appointments.Repositories;
using CC.AppointmentService.Domain.Feedbacks;
using CC.AppointmentService.Domain.Feedbacks.Repositories;
using CC.Shared.Domain;
using CC.Shared.Domain.TimeRanges;
using NSubstitute;
using Shouldly;

namespace CC.AppointmentTests;

public class CreateFeedbackTest
{
    private readonly IAppointmentsRepository _appointments = Substitute.For<IAppointmentsRepository>();
    private readonly IFeedbackRepository _feedbacks = Substitute.For<IFeedbackRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly TimeProvider _timeProvider = Substitute.For<TimeProvider>();

    private readonly DateTimeOffset _fixedNow = new(2026, 04, 01, 10, 0, 0, TimeSpan.Zero);
    private readonly Guid _appointmentId = Guid.NewGuid();
    private readonly Guid _clientId = Guid.NewGuid();

    private CreateFeedbackUseCase BuildSut(DateTimeOffset endedAt, AppointmentStatus status = AppointmentStatus.Completed)
    {
        _timeProvider.GetUtcNow().Returns(_fixedNow);

        var appointment = new Appointment(_appointmentId)
        {
            EndedAt = endedAt.UtcDateTime,
            ClientId = _clientId,
            PractitionerId = Guid.NewGuid(),
            TimeSlot = new DateTimeRange(
                new DateTime(2026, 03, 29, 8, 0, 0, DateTimeKind.Utc),
                new DateTime(2026, 03, 29, 9, 0, 0, DateTimeKind.Utc)),
            Status = status,
            ClientSnapshot = new ClientSnapshot { FullName = new FullName("Test", "Client", null) },
            PractitionerSnapshot = new PractitionerSnapshot { FullName = new FullName("Test", "Doctor", null) },
            CallUrl = null
        };

        _appointments.GetByIdAsync(_appointmentId).Returns(appointment);
        _feedbacks.ExistsByAppointmentIdAsync(_appointmentId).Returns(false);

        return new CreateFeedbackUseCase(_unitOfWork, _appointments, _feedbacks, _timeProvider);
    }

    [Fact]
    public async Task ShouldFailWhenTooLate()
    {
        var sut = BuildSut(_fixedNow.AddDays(-3));

        var result = await sut.Handle(new CreateFeedback
        {
            AppointmentId = _appointmentId,
            ClientId = _clientId,
            ComfortScore = 5,
            EmpathyScore = 5,
            ProfessionalismScore = 5,
            Tags = []
        }, default);
        
        result.IsSuccess.ShouldBeFalse();
        object? code;
        result.Errors.ShouldContain(e => e.Metadata.TryGetValue("ErrorCode", out code) && code.ToString() == "F101");
        await _feedbacks.DidNotReceive().AddAsync(Arg.Any<Feedback>());
        await _unitOfWork.DidNotReceive().SaveAsync(Arg.Any<CancellationToken>());
    }
}