using CC.AppointmentService.Api.Contracts;
using CC.AppointmentService.Application.UseCases;
using CC.AppointmentService.Application.UseCases.SessionReview.Creation;
using CC.AppointmentService.Domain.Reviews.ValueObjects;
using Mediator;

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
            ReviewComment = ReviewComment.From(request.ReviewComment ?? string.Empty)
        });
        
        return Results.Ok(result);
    }
}
