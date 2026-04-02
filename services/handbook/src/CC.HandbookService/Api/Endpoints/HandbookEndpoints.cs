namespace CC.HandbookService.Api.Endpoints;

public static class HandbookEndpoints
{
    extension(IEndpointRouteBuilder builder)
    {
        public void MapHandbookEndpoints()
        {
            var v1Group = builder.MapGroup("api/v1/{handbook}/upload");

            v1Group.MapPost("", UploadHandbookEndpoint.Handle);
        }
    }
}