using CC.Shared.Domain;

namespace CC.AppointmentService.Domain.Feedback;

public class Review(
    Guid id,
    Guid appointmentId,
    ComfortScore comfortScore,
    ProfessionalismScore professionalismScore,
    Feedback.EmpathyScore empathyScore,
    DateTime createdAt) : AggregationRoot<Guid>(id)
{
    public Guid AppointmentId { get; private set; } = appointmentId;
    public ComfortScore ComfortScore { get; private set; } = comfortScore;
    public ProfessionalismScore ProfessionalismScore { get; private set; } = professionalismScore;
    public Feedback.EmpathyScore EmpathyScore { get; private set; } = empathyScore;
    public DateTime CreatedAt { get; private set; } = createdAt;
    public ModerationStatus ModerationStatus { get; private set; } = ModerationStatus.Pending;
    public IReadOnlyCollection<Guid> Tags { get; private set; } = [];
    
    public void AddTags(IEnumerable<Guid> tags) =>
        Tags = Tags
            .Union(tags)
            .ToArray();
}
