namespace CC.NotificationService.Api.Endpoints.WebNotifications;

public static class WebNotificationsEndpoints
{
    extension(IEndpointRouteBuilder builder)
    {
        public void MapWebNotificationsEndpoints()
        {
            var v1Group = builder.MapGroup("api/v1/web-notifications");
        }
    }
}