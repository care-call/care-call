using CC.NotificationService.Domain.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CC.NotificationService.Api.Endpoints.Templates;

public class GetTemplateByIdEndpoint
{
    public static async Task<IResult> Handle(
        [FromServices] ITemplateRepository templateRepository,Guid Id)
    {
        var template = await templateRepository.GetByIdAsync(Id);
        if (template == null)
            return Results.NotFound();
        
        return Results.Ok(template);
    }
}