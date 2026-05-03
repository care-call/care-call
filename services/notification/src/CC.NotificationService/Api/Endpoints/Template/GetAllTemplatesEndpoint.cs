using CC.Common.Models;
using CC.NotificationService.Api.Dto;
using CC.NotificationService.Application.Dependencies;

namespace CC.NotificationService.Api.Endpoints.Template;

public class GetAllTemplatesEndpoint
{
    public static async Task<IResult> Handle(
         ITemplateQuery templateQueryService,
         [AsParameters] ItemFilterTemplate itemFilterTemplate,
         int pageNum = 1,
         int pageSize = 10)
    {
        if (pageNum < 1 || pageSize < 1)
            Results.BadRequest("Параметры страницы и Размер страницы должны быть больше 0.");        
        
        var templates = await templateQueryService.GetAllAsync(itemFilterTemplate.Key, itemFilterTemplate.IsActive, pageNum, pageSize);

        var result = templates
         .Items      
         .Select(t => new GetTemplateResponse(t.Id, t.Key, t.IsActive))       
         .ToList();

        return Results.Ok(new PagedResult<GetTemplateResponse>(result, new PageInfo(pageNum, pageSize), result.Count, templates.TotalPages));
    }
}