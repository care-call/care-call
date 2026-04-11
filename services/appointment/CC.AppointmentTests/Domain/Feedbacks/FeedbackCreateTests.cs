using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Feedbacks;
using FluentResults;
using Shouldly;

namespace CC.AppointmentTests.Domain.Feedbacks;

public class FeedbackCreateTests
{
    private static readonly DateTime Now = new(2026, 04, 01, 10, 0, 0, DateTimeKind.Utc);
    private static readonly Guid AppointmentId = Guid.NewGuid();
    private static readonly Guid ClientId = Guid.NewGuid();

    private static Appointment CompletedAppointment(DateTime endedAt)
    {
        var appointment = AppointmentBuilder.Create(AppointmentId, ClientId)
            .WithStatus(AppointmentStatus.InProgress)
            .Build();
        appointment.Complete(endedAt);
        return appointment;
    }

    private static Result<Feedback> CreateFeedback(Appointment appointment) =>
        Feedback.Create(
            appointment,
            ComfortScore.From(5),
            ProfessionalismScore.From(5),
            EmpathyScore.From(5),
            Now);

    [Fact]
    public void Feedback_for_incomplete_appointment_is_rejected()
    {
        var appointment = AppointmentBuilder.Create(AppointmentId, ClientId)
            .WithStatus(AppointmentStatus.Planned)
            .Build();

        var result = CreateFeedback(appointment);

        result.ShouldHaveErrorCode("F102");
    }

    [Fact]
    public void Feedback_after_creation_window_is_rejected()
    {
        var appointment = CompletedAppointment(
            endedAt: Now.AddDays(-(Feedback.CreationWindowDays + 1)));

        var result = CreateFeedback(appointment);

        result.ShouldHaveErrorCode("F101");
    }

    [Fact]
    public void Feedback_within_creation_window_is_accepted()
    {
        var appointment = CompletedAppointment(endedAt: Now.AddDays(-1));

        var result = CreateFeedback(appointment);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void Feedback_on_the_last_day_of_creation_window_is_accepted()
    {
        // now == endedAt + CreationWindowDays → граница включена (>), не (>=)
        var appointment = CompletedAppointment(
            endedAt: Now.AddDays(-Feedback.CreationWindowDays));

        var result = CreateFeedback(appointment);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void Feedback_includes_tags_when_they_are_provided()
    {
        var appointment = CompletedAppointment(endedAt: Now.AddDays(-1));
        var tags = new[] { Guid.NewGuid(), Guid.NewGuid() };

        var result = Feedback.Create(
            appointment,
            ComfortScore.From(5),
            ProfessionalismScore.From(5),
            EmpathyScore.From(5),
            Now,
            tags);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Tags.ShouldBe(tags, ignoreOrder: true);
    }
}