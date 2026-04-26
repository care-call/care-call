using CC.NotificationService.Domain.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CC.NotificationService.Api.Endpoints.Templates;

public class GetAllTemplatesEndpoint
{
    public static async Task<IResult> Handle(
         [FromServices] ITemplateRepository repository,
         [FromQuery] string key,
         [FromQuery] bool isActive,
         [FromQuery] int page = 1,
         [FromQuery] int pageSize = 10)
    {
        if(page < 1 || pageSize < 1)
        {
            return Results.BadRequest("Page and pageSize must be greater than 0.");
        }

        var templates = await repository.GetAllAsync(key, isActive, page, pageSize);
        return Results.Ok(templates);
    }
}