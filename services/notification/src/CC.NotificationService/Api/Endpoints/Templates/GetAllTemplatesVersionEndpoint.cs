using CC.Common.Models;
using CC.NotificationService.Domain.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CC.NotificationService.Api.Endpoints.TemplateVersions;

public class GetAllTemplatesVersionsEndpoint
{
    public static async Task<IResult> Handle(
       [FromServices] ITemplateVersionRepository repository,
       [FromQuery] bool IsActive,
       [FromQuery] int page = 1,
       [FromQuery] int pageSize = 10)
    {
        if(page < 1 || pageSize < 1)
        {
            return Results.BadRequest("Page and PageSize must be greater than 0.");
        }

        var result = await repository.GetAllAsync(IsActive, page, pageSize);
        return Results.Ok(result);
    }
}