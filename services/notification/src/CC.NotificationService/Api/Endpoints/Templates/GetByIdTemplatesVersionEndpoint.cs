using CC.NotificationService.Domain.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CC.NotificationService.Api.Endpoints.TemplateVersions;

public class GetTemplateVersionByIdEndpoint
{
    public static async Task<IResult> Handle([FromServices] ITemplateVersionRepository templateVersionRepository, Guid Id)
    {
        var templateVersion = await templateVersionRepository.GetByIdAsync(Id);
        if (templateVersion == null)
        {
            return Results.NotFound();
        }
        return Results.Ok(templateVersion);
    }
}