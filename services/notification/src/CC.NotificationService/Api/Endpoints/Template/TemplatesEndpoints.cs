using CC.NotificationService.Api.Endpoints.TemplateVersions;
using CC.NotificationService.Domain.TemplateVersions;

namespace CC.NotificationService.Api.Endpoints.Template;

public static class TemplatesEndpoints
{
    extension(IEndpointRouteBuilder builder)
    {
        public void MapTemplateEndpoints()
        {
           var v1Group = builder.MapGroup("/api/v1/admin/templates");
           v1Group.MapPost("", CreateTemplateEndpoint.Handle);
           v1Group.MapGet("", GetAllTemplatesEndpoint.Handle);        
           v1Group.MapGet("/{id:guid}", GetTemplateByIdEndpoint.Handle);

           v1Group.MapPost("/versions", CreateTemplateVersionEndpoint.Handle);
           v1Group.MapGet("/{key}/versions", GetAllTemplatesVersionsEndpoint.Handle);
           v1Group.MapGet("/versions/{id:guid}", GetTemplatesVersionByIdEndpoint.Handle);
        }
    }
}