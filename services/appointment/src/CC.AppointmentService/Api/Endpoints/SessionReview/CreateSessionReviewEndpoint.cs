using CC.AppointmentService.Api.Contracts;
using CC.AppointmentService.Application.UseCases.SessionReview.Creation;
using CC.AppointmentService.Domain.Reviews.ValueObjects;
using Mediator;
using TagValue = CC.AppointmentService.Domain.Reviews.ValueObjects.Tags.Tag;

namespace CC.AppointmentService.Api.Endpoints.SessionReview;

public static class CreateSessionReviewEndpoint
{
    public static async Task<IResult> Handle(CreateSessionReviewRequest request, IMediator mediator)
    {
        var result = await mediator.Send(new CreateSessionReview
        {
            AppointmentId = request.AppointmentId,
            UserId = request.UserId,
            EmpathyRating = EmpathyRating.From(request.EmpathyRating),
            ProfessionalismRating = ProfessionalismRating.From(request.ProfessionalismRating),
            ComfortRating = ComfortRating.From(request.ComfortRating),
            Tags = request.Tags.Select(TagValue.From).Distinct().ToArray(),
            ReviewComment = string.IsNullOrWhiteSpace(request.ReviewComment)
                ? null
                : ReviewComment.From(request.ReviewComment)
        });
        
        return Results.Ok(result);
    }
}
