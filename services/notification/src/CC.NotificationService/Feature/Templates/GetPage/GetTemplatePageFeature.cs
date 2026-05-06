using CC.Common.Models;
using CC.NotificationService.Feature.Templates.Mapper;
using CC.NotificationService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wolverine.Http;

namespace CC.NotificationService.Feature.Templates.GetPage;

public class GetTemplatePageFeature
{
    [ProducesResponseType<PagedResult<TemplateDto>>(200)]
    [WolverineGet("api/v1/templates")]
    public static async Task<IResult> Handle(
        string key,
        bool isActive,
        int pageNum,
        int pageSize,
        DatabaseContext databaseContext)
    {
        if (pageNum < 1 || pageSize < 1)
            Results.BadRequest("Параметры страницы и Размер страницы должны быть больше 0.");   
        
        var query = databaseContext.Templates
            .Where(t => t.IsActive == isActive && t.Key.StartsWith(key)); 

        var totalItems = await query.CountAsync();
        var items = await TemplateDtoMapper
            .MapToDto(query
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize))
            .ToListAsync();

        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
        return Results.Ok(new PagedResult<TemplateDto>(items, new PageInfo(pageNum, pageSize), totalItems, totalPages));
    }
}