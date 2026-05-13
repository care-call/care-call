using CC.Common.Models;
using CC.Common.Pagination;
using CC.NotificationService.Feature.Templates.Mapper;
using CC.NotificationService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Wolverine.Http;

namespace CC.NotificationService.Feature.Templates.GetVersionsPage;

public class GetTemplateVersionsPageFeature
{
    [ProducesResponseType<PagedResult<TemplateVersionDto>>(200)]
    [WolverineGet("api/v1/templates/{key}/versions")]
    public static async Task<IResult> Handle(
        string key,
        bool isActive,
        int pageNum,
        int pageSize,
        DatabaseContext databaseContext)
    {
        var pageInfo = new PageInfo(pageNum, pageSize);
        if (pageInfo.IsInvalid(out var validationErrors))
            return Results.ValidationProblem(validationErrors);

        var query = databaseContext.TemplateVersions
            .Where(t => t.IsActive == isActive && t.TemplateKey.StartsWith(key));

        var pagedResult = await TemplateVersionDtoMapper
            .MapToDto(query)
            .ToPagedResultAsync(pageInfo);

        return Results.Ok(pagedResult);
    }
}