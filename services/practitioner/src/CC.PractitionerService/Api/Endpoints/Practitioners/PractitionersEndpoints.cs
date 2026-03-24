namespace CC.PractitionerService.Api.Endpoints.Practitioners;

public static class PractitionersEndpoints
{
    extension(IEndpointRouteBuilder builder)
    {
        public void MapPractitionersEndpoints()
        {
            var v1Group = builder.MapGroup("api/v1/practitioners");
            v1Group.MapPost("", FindPractitionersEndpoint.Handle);
        }
    }
}
