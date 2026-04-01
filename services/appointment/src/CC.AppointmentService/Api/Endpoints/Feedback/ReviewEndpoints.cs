namespace CC.AppointmentService.Api.Endpoints.Feedback;

public static class ReviewEndpoints
{
    extension(IEndpointRouteBuilder builder)
    {
        public void MapReviewsEndpoints()
        {
            var v1Group = builder.MapGroup("api/v1/reviews");
            v1Group.MapPost("", FeedbackReviewEndpoint.Handle);
        }
    }
}