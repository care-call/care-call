using CC.AppointmentService.Api.Contracts.Feedback;
using CC.AppointmentService.Api.Endpoints.Feedback.Mapper;
using FluentResults;
using Wolverine;

namespace CC.AppointmentService.Api.Endpoints.Feedback;

public static class CreateFeedbackEndpoint
{
    public static async Task<IResult> Handle(
        CreateFeedbackRequest feedbackRequest,
        IMessageBus bus,
        CancellationToken ct)
    {
        var result = await bus.InvokeAsync<Result>(FeedbackMapper.ToUseCase(feedbackRequest), ct);

        return result.IsSuccess
            ? Results.Ok()
            : Results.BadRequest(result.Errors);
    }
}