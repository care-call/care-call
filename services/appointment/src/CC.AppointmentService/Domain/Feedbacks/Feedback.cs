using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Errors;
using CC.Shared.Domain;
using FluentResults;

namespace CC.AppointmentService.Domain.Feedbacks;

public class Feedback(
    Guid id,
    Guid appointmentId,
    ComfortScore comfortScore,
    ProfessionalismScore professionalismScore,
    EmpathyScore empathyScore,
    DateTime createdAt) : AggregationRoot<Guid>(id)
{
    public const int CreationWindowDays = 2;

    public Guid AppointmentId { get; private set; } = appointmentId;
    public ComfortScore ComfortScore { get; private set; } = comfortScore;
    public ProfessionalismScore ProfessionalismScore { get; private set; } = professionalismScore;
    public EmpathyScore EmpathyScore { get; private set; } = empathyScore;
    public DateTime CreatedAt { get; private set; } = createdAt;
    public ModerationStatus ModerationStatus { get; private set; } = ModerationStatus.Pending;
    public IReadOnlyCollection<Guid> Tags { get; private set; } = [];

    public static Result<Feedback> Create(
        Appointment appointment,
        ComfortScore comfortScore,
        ProfessionalismScore professionalismScore,
        EmpathyScore empathyScore,
        DateTime now,
        IReadOnlyCollection<Guid>? tags = null)
    {
        if (appointment.Status != AppointmentStatus.Completed)
            return Result.Fail(FeedbackError.AppointmentNotCompleted);
        if (now > appointment.EndedAt!.Value.AddDays(CreationWindowDays))
            return Result.Fail(FeedbackError.TooLate);

        var feedback = new Feedback(
            Guid.CreateVersion7(),
            appointment.Id,
            comfortScore,
            professionalismScore,
            empathyScore,
            now);

        if (tags is not null)
            feedback.AddTags(tags);

        return feedback;
    }

    public void AddTags(IEnumerable<Guid> tags) => Tags = Tags.Union(tags).ToArray();
}
