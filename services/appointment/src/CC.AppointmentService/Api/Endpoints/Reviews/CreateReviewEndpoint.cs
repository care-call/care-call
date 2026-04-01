using CC.AppointmentService.Api.Contracts.Reviews;
using CC.AppointmentService.Api.Endpoints.Reviews.Mapper;
using Mediator;

namespace CC.AppointmentService.Api.Endpoints.Reviews;

public static class CreateReviewEndpoint
{
    public static async Task<IResult> Handle(
        CreateReviewRequest reviewRequest,
        IMediator mediator)
    {
        var result = await mediator.Send(ReviewMapper.ToUseCase(reviewRequest), CancellationToken.None);

        return result.IsSuccess
            ? Results.Ok()
            : Results.BadRequest(result.Errors);
    }
}