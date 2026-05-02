using CC.Common.Models;
using CC.NotificationService.Api.Dto;
using CC.NotificationService.Application.Dependencies;
using CC.NotificationService.Domain;

namespace CC.NotificationService.Api.Endpoints.TemplateVersions;

public class GetAllTemplatesVersionsEndpoint
{
    public static async Task<IResult> Handle(
        ITemplateVersionQuery templateVersionQueryService,
        bool isActive,
        int pageNum = 1,
        int pageSize = 10)
    {  
        if (pageNum < 1 || pageSize < 1)
            return Results.BadRequest("Параметры страницы и Размер страницы должны быть больше 0.");
        
        var result = await templateVersionQueryService.GetAllAsync(isActive, pageNum, pageSize);

        var templates = result
          .Items
          .Select(tv => new GetTemplateVersionResponse(tv.Id, tv.TemplateKey, tv.Version, tv.IsActive, tv.Channels, tv.CreatedAt))
          .ToList();

        return Results.Ok(new PagedResult<GetTemplateVersionResponse>(templates, new PageInfo(pageNum, pageSize), templates.Count, result.TotalPages));
    }
}