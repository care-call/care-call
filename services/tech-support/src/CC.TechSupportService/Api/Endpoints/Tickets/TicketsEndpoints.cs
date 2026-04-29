namespace CC.TechSupportService.API.Endpoints.Tickets;

public static class TicketsEndpoints
{
    extension(IEndpointRouteBuilder builder)
    {
        public void MapTicketEndpoints()
        {
            var v1Group = builder.MapGroup("api/v1/tickets");
            v1Group.MapPost("", CreateTicketEndpoint.Handle);
        }
    }
}