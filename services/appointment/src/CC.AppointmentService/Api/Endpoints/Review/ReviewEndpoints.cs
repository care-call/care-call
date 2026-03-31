namespace CC.AppointmentService.Api.Endpoints.Review;

public static class ReviewEndpoints
{
    extension(IEndpointRouteBuilder builder)
    {
        public void MapReviewsEndpoints()
        {
            var v1Group = builder.MapGroup("api/v1/reviews");
            v1Group.MapPost("", CreateReviewEndpoint.Handle);
        }
    }
}