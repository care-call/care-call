using CC.NotificationService.Domain;
using CC.NotificationService.Domain.Dto;
using CC.NotificationService.Domain.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CC.NotificationService.Api.Endpoints.Templates;

public class CreateTemplateEndpoint
{
    public static async Task<IResult> Handle(
       [FromServices] ITemplateRepository repository,
       [FromBody] TemplateDto templateDto)
    {
        var template = new Template
        {
            Key = templateDto.key,
            IsActive = templateDto.isActive,
            CreateAt = DateTime.UtcNow
        };
        var createdTemplate = await repository.CreateAsync(template);
        return Results.Ok(template);
    }
}