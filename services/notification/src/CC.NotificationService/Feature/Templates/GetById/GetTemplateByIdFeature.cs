using CC.NotificationService.Feature.Templates.Mapper;
using CC.NotificationService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wolverine.Http;

namespace CC.NotificationService.Feature.Templates.GetById;

public class GetTemplateByIdFeature
{
    [ProducesResponseType<TemplateDto>(200)]
    [WolverineGet("api/v1/templates/{id}")]
    public static async Task<IResult> Handle(
        Guid id,
        DatabaseContext databaseContext)
    {
        var template = await databaseContext.Templates.FirstOrDefaultAsync(t => t.Id == id);
        if (template is null)
            return Results.NotFound("Шаблон не найден");
        
        var result = TemplateDtoMapper.From(template);
        return Results.Ok(result);
    }
}