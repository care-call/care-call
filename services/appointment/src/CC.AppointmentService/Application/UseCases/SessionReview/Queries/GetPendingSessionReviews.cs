using CC.AppointmentService.Domain.Reviews.Repositories;
using Mediator;

namespace CC.AppointmentService.Application.UseCases.SessionReview.Queries;

public sealed record GetPendingSessionReviews : IRequest<IReadOnlyCollection<PendingSessionReviewDto>>;

public sealed record PendingSessionReviewDto(
    Guid Id,
    Guid AppointmentId,
    int EmpathyRating,
    int ProfessionalismRating,
    int ComfortRating,
    IReadOnlyCollection<Guid> Tags,
    string? ReviewComment);

public class GetPendingSessionReviewsUseCase(
    ISessionReviewRepository sessionReviewRepository)
    : IRequestHandler<GetPendingSessionReviews, IReadOnlyCollection<PendingSessionReviewDto>>
{
    public async ValueTask<IReadOnlyCollection<PendingSessionReviewDto>> Handle(
        GetPendingSessionReviews query,
        CancellationToken cancellationToken)
    {
        var reviews = await sessionReviewRepository.GetPendingAsync(cancellationToken);

        return reviews.Select(x => new PendingSessionReviewDto(
            x.Id,
            x.AppointmentId,
            x.EmpathyRating.Value,
            x.ProfessionalismRating.Value,
            x.ComfortRating.Value,
            x.Tags.Select(tag => tag.Value).ToArray(),
            x.ReviewComment.HasValue ? x.ReviewComment.Value.Value : null
        )).ToArray();
    }
}
