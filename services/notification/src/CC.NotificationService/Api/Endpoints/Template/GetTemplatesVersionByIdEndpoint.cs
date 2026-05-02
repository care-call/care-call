using CC.NotificationService.Domain.Interfaces;

namespace CC.NotificationService.Api.Endpoints.TemplateVersions;

public class GetTemplatesVersionByIdEndpoint
{
    public static async Task<IResult> Handle(
       Guid id,
       ITemplateVersionRepository templateVersionRepository)
    {
        var templateVersion = await templateVersionRepository.GetByIdAsync(id);
        if (templateVersion == null)       
            return Results.NotFound();
        
        return Results.Ok(templateVersion);
    }
}