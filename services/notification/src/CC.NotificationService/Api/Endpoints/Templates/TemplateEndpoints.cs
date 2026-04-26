using CC.NotificationService.Api.Endpoints.TemplateVersions;

namespace CC.NotificationService.Api.Endpoints.Templates;

public static class TemplateEndpoints
{
    extension(IEndpointRouteBuilder builder)
    {
        public void MapTemplateEndpoints()
        {
           var v1Group = builder.MapGroup("/api/admin/templates");
           v1Group.MapPost("/", CreateTemplateEndpoint.Handle);
           v1Group.MapGet("/",  GetAllTemplatesEndpoint.Handle);
           
           v1Group.MapGet("/{key}", GetTemplateByIdEndpoint.Handle);

           v1Group.MapPost("/versions", CreateTemplateVersionEndpoint.Handle);
           v1Group.MapGet("/versions", GetAllTemplatesVersionsEndpoint.Handle);
           v1Group.MapGet("/versions/{id:guid}", GetTemplateVersionByIdEndpoint.Handle);
        }
    }
}