namespace CC.AppointmentService.Api.Endpoints.SessionReview;

public static class SessionReviewEndpoints
{
    extension(IEndpointRouteBuilder builder)
    {
        public void MapSessionReviewEndpoint()
        {
            var v1Group = builder.MapGroup("/api/v1/sessionreview");
            v1Group.MapPost("", CreateSessionReviewEndpoint.Handle);
            v1Group.MapGet("pending", GetPendingSessionReviewsEndpoint.Handle);
        }
    }
}
