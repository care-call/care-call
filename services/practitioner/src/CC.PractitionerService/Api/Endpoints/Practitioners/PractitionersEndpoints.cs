namespace CC.PractitionerService.Api.Endpoints.Practitioners;

public static class PractitionersEndpoints
{
    extension(IEndpointRouteBuilder builder)
    {
        public void MapPractitionersEndpoints()
        {
            var v1Group = builder.MapGroup("api/v1/practitioners")
                .WithTags("Practitioners");

            v1Group.MapPost("match", MatchPractitionersEndpoint.Handle)
                .WithSummary("Подобрать практиканта для записи");
        }
    }
}
