namespace CC.AppointmentService.Api.Endpoints.Feedback;

public static class FeedbackEndpoints
{
    extension(IEndpointRouteBuilder builder)
    {
        public void MapFeedbackEndpoints()
        {
            var v1Group = builder.MapGroup("api/v1/feedbacks");
            v1Group.MapPost("", CreateFeedbackEndpoint.Handle);
        }
    }
}