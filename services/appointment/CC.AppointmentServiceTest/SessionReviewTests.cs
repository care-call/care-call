using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Reviews;
using CC.AppointmentService.Domain.Reviews.Rules;
using CC.AppointmentService.Domain.Reviews.ValueObjects;
using CC.Shared.Domain;
using CC.Shared.Domain.TimeRanges;
using Shouldly;

namespace CC.AppointmentServiceTest;

public class SessionReviewRulesTests
{
    [Fact]
    public void Review_is_not_available_for_planned_appointment()
    {
        var appointment = CreateScheduledAppointment(3);

        var result = SessionReviewRules.IsStatusCreatable(appointment);

        result.ShouldBeFalse();
    }

    [Fact]
    public void Review_is_not_available_for_cancelled_appointment()
    {
        var appointment = CreateScheduledAppointment(3, AppointmentStatus.Cancelled);

        var result = SessionReviewRules.IsStatusCreatable(appointment);

        result.ShouldBeFalse();
    }

    [Fact]
    public void Review_is_available_for_completed_appointment_within_two_days()
    {
        var appointment = CreateCompletedAppointment(1);

        var result = SessionReviewRules.IsWithinAllowedReviewing(appointment);

        result.ShouldBeTrue();
    }

    [Fact]
    public void Review_is_not_available_for_completed_appointment_after_two_days()
    {
        var appointment = CreateCompletedAppointment(3);

        var result = SessionReviewRules.IsWithinAllowedReviewing(appointment);

        result.ShouldBeFalse();
    }

    [Fact]
    public void Review_is_not_available_for_completed_appointment_without_end_date()
    {
        var appointment = CreateAppointment(
            DateTime.Now.AddHours(-2),
            DateTime.Now.AddHours(-1),
            AppointmentStatus.Completed);

        var result = SessionReviewRules.IsWithinAllowedReviewing(appointment);

        result.ShouldBeFalse();
    }

    private static Appointment CreateCompletedAppointment(int endedDaysAgo)
    {
        var endedAt = DateTime.Now.Subtract(TimeSpan.FromDays(endedDaysAgo));
        var startedAt = endedAt.AddHours(-1);

        var appointment = CreateAppointment(startedAt, endedAt, AppointmentStatus.InProgress);
        appointment.Complete(endedAt);

        return appointment;
    }

    private static Appointment CreateScheduledAppointment(int scheduledDaysAhead, AppointmentStatus status = AppointmentStatus.Planned)
    {
        var startedAt = DateTime.Today;
        var endedAt = startedAt.AddDays(scheduledDaysAhead);

        return CreateAppointment(startedAt, endedAt, status);
    }

    private static Appointment CreateAppointment(DateTime startedAt, DateTime endedAt, AppointmentStatus status)
    {
        return new Appointment(Guid.NewGuid())
        {
            ClientId = Guid.Empty,
            PractitionerId = Guid.Empty,
            TimeSlot = new DateTimeRange(startedAt, endedAt),
            Status = status,
            ClientSnapshot = new ClientSnapshot
            {
                FullName = new FullName("Client", "Client", "")
            },
            PractitionerSnapshot = new PractitionerSnapshot
            {
                FullName = new FullName("Practitioner", "Practitioner", "")
            }
        };
    }
}

public class SessionReviewStateTests
{
    [Fact]
    public void New_review_is_created_with_pending_status()
    {
        var review = CreateReview();

        review.Status.ShouldBe(SessionReviewStatus.Pending);
    }

    [Fact]
    public void Review_is_published_after_mark_published()
    {
        var review = CreateReview();

        review.MarkPublished();

        review.Status.ShouldBe(SessionReviewStatus.Published);
    }

    [Fact]
    public void Review_is_moved_to_manual_review_after_mark_manual_review()
    {
        var review = CreateReview();

        review.MarkManualReview();

        review.Status.ShouldBe(SessionReviewStatus.ManualReview);
    }

    private static SessionReview CreateReview()
    {
        return new SessionReview(Guid.NewGuid())
        {
            AppointmentId = Guid.NewGuid(),
            EmpathyRating = EmpathyRating.From(5),
            ProfessionalismRating = ProfessionalismRating.From(5),
            ComfortRating = ComfortRating.From(5),
            ReviewComment = null
        };
    }
}
