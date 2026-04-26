using CC.NotificationService.Domain.Dto;
using CC.NotificationService.Domain.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CC.NotificationService.Api.Endpoints.TemplateVersion;

public class DeleteByIdTemplatesVersionEndpoint
{
    public static async Task<IResult> Handle(
       [FromServices] ITemplateVersionRepository repository,
       [FromBody] TemplateVersionDto templateVersionDto, Guid Id)
    {
        var template = await repository.GetByIdAsync(Id);       
        if (template == null)
           return Results.NotFound();

        await repository.DeleteByIdAsync(Id);
        return Results.NoContent();
    }
}