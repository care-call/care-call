using CC.AppointmentService.Api.Contracts.Feedback;
using CC.AppointmentService.Api.Endpoints.Feedback.Mapper;
using Mediator;

namespace CC.AppointmentService.Api.Endpoints.Feedback;

public static class CreateFeedbackEndpoint
{
    public static async Task<IResult> Handle(
        CreateFeedbackRequest feedbackRequest,
        IMediator mediator,
        CancellationToken ct)
    {
        var result = await mediator.Send(FeedbackMapper.ToUseCase(feedbackRequest), ct);

        return result.IsSuccess
            ? Results.Ok()
            : Results.BadRequest(result.Errors);
    }
}