using CC.Shared.Domain;

namespace CC.AppointmentService.Domain.Reviews;

public class Review(Guid id) : AggregationRoot<Guid>(id)
{
    public required Guid AppointmentId { get; init; }
    public required ComfortScore ComfortScore { get; init; }
    public required ProfessionalismScore ProfessionalismScore { get; init; }
    public required EmpathyScore EmpathyScore { get; init; }
    public required DateTime CreatedAt { get; init; }
    public ModerationStatus ModerationStatus { get; private set; } = ModerationStatus.Pending;
    public IReadOnlyCollection<Guid> Tags { get; private set; } = [];
    
    public void AddTags(IEnumerable<Guid> tags)
    {
        Tags = Tags
            .Union(tags)
            .ToArray();
    }

    public void Approve()
    {
        ModerationStatus = ModerationStatus.Approved;
    }

    public void Reject()
    {
        ModerationStatus = ModerationStatus.Rejected;
    }
}
