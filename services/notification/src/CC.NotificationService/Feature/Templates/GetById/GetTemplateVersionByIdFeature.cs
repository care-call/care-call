using CC.NotificationService.Feature.Templates.Mapper;
using CC.NotificationService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wolverine.Http;

namespace CC.NotificationService.Feature.Templates.GetById;

public class GetTemplateVersionByIdFeature
{
    [ProducesResponseType<TemplateVersionDto>(200)]
    [WolverineGet("api/v1/templates/versions/{id}")]
    public static async Task<IResult> Handle(
        Guid id,
        DatabaseContext databaseContext)
    {
        var templateVersion = await databaseContext.TemplateVersions.FirstOrDefaultAsync(t => t.Id == id);
        if (templateVersion is null)
            return Results.NotFound("Версия шаблона не найдена");
        
        var result = TemplateVersionDtoMapper.From(templateVersion);
        return Results.Ok(result);
    }
}