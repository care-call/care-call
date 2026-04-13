using CC.AppointmentService.Application.Dependencies.UnitOfWork;
using CC.AppointmentService.Application.UseCases.Feedbacks.Creation;
using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Appointments.Repositories;
using CC.AppointmentService.Domain.Feedbacks;
using CC.AppointmentService.Domain.Feedbacks.Repositories;
using FluentResults;
using NSubstitute;
using Shouldly;

namespace CC.AppointmentTests.UseCases.Feedbacks;

public class CreateFeedbackUseCaseTests
{
    private readonly IAppointmentsRepository _appointments = Substitute.For<IAppointmentsRepository>();
    private readonly IFeedbackRepository _feedbacks = Substitute.For<IFeedbackRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private readonly DateTime _now = new(2026, 04, 01, 10, 0, 0, DateTimeKind.Utc);
    private readonly Guid _appointmentId = Guid.NewGuid();
    private readonly Guid _clientId = Guid.NewGuid();

    private CreateFeedback DefaultCommand => new()
    {
        AppointmentId = _appointmentId,
        ClientId = _clientId,
        ComfortScore = 5,
        EmpathyScore = 5,
        ProfessionalismScore = 5,
        Tags = []
    };

    private ValueTask<Result> Act(CreateFeedback? command = null) =>
        CreateFeedbackUseCase.Handle(
            command ?? DefaultCommand,
            _unitOfWork, _appointments, _feedbacks,
            _now, CancellationToken.None);

    private Appointment MakeCompletedAppointment(Guid? clientId = null)
    {
        var appointment = AppointmentBuilder.Create(_appointmentId, clientId ?? _clientId)
            .WithStatus(AppointmentStatus.InProgress)
            .Build();
        appointment.Complete(_now.AddDays(-1));
        return appointment;
    }

    [Fact]
    public async Task Feedback_creation_for_nonexistent_appointment_is_rejected()
    {
        _appointments.GetByIdAsync(_appointmentId).Returns((Appointment?)null);

        var result = await Act();

        result.ShouldHaveErrorCode("A202");
        await _feedbacks.DidNotReceive().AddAsync(Arg.Any<Feedback>());
        await _unitOfWork.DidNotReceive().SaveAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Feedback_creation_for_another_clients_appointment_is_rejected()
    {
        _appointments.GetByIdAsync(_appointmentId)
            .Returns(MakeCompletedAppointment(clientId: Guid.NewGuid()));

        var result = await Act();

        result.ShouldHaveErrorCode("A202");
        await _unitOfWork.DidNotReceive().SaveAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Client_cannot_leave_two_feedbacks_for_the_same_appointment()
    {
        _appointments.GetByIdAsync(_appointmentId).Returns(MakeCompletedAppointment());
        _feedbacks.ExistsByAppointmentIdAsync(_appointmentId).Returns(true);

        var result = await Act();

        result.ShouldHaveErrorCode("F103");
        await _feedbacks.DidNotReceive().AddAsync(Arg.Any<Feedback>());
        await _unitOfWork.DidNotReceive().SaveAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Valid_feedback_is_saved_and_committed()
    {
        _appointments.GetByIdAsync(_appointmentId).Returns(MakeCompletedAppointment());
        _feedbacks.ExistsByAppointmentIdAsync(_appointmentId).Returns(false);

        var result = await Act();

        result.IsSuccess.ShouldBeTrue();
        await _feedbacks.Received(1).AddAsync(Arg.Any<Feedback>());
        await _unitOfWork.Received(1).SaveAsync(Arg.Any<CancellationToken>());
    }
}
