using CC.AppointmentService.Application.UseCases.SessionReview.Queries;
using Mediator;

namespace CC.AppointmentService.Api.Endpoints.SessionReview;

public static class GetPendingSessionReviewsEndpoint
{
    public static async Task<IResult> Handle(
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetPendingSessionReviews(), cancellationToken);
        return Results.Ok(response);
    }
}
