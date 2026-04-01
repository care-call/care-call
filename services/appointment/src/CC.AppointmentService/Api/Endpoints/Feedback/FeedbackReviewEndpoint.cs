using CC.AppointmentService.Api.Contracts.Feedback;
using CC.AppointmentService.Api.Endpoints.Feedback.Mapper;
using Mediator;

namespace CC.AppointmentService.Api.Endpoints.Feedback;

public static class FeedbackReviewEndpoint
{
    public static async Task<IResult> Handle(
        CreateFeedbackRequest feedbackRequest,
        IMediator mediator)
    {
        var result = await mediator.Send(FeedbackMapper.ToUseCase(feedbackRequest), CancellationToken.None);

        return result.IsSuccess
            ? Results.Ok()
            : Results.BadRequest(result.Errors);
    }
}