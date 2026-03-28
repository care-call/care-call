using CC.AppointmentService.Domain.Reviews.ValueObjects;
using CC.Shared.Domain;

namespace CC.AppointmentService.Domain.Reviews;

public sealed class SessionReview(Guid id) : AggregationRoot<Guid>(id)
{
    public required Guid AppointmentId { get; init; }
    public SessionReviewStatus Status { get; private set; } = SessionReviewStatus.Pending;
    
    public required EmpathyRating EmpathyRating { get; init; }
    public required ProfessionalismRating ProfessionalismRating { get; init; }
    public required ComfortRating ComfortRating { get; init; }
    
    public required ReviewComment? ReviewComment { get; init; }

    public void MarkPublished()
    {
        Status = SessionReviewStatus.Published;
    }
    public void MarkManualReview()
    {
        Status = SessionReviewStatus.ManualReview;
    }
}