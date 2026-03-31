using CC.AppointmentService.Api.Contracts.Review;
using CC.AppointmentService.Api.Endpoints.Review.Mapper;
using Mediator;

namespace CC.AppointmentService.Api.Endpoints.Review;

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