using CC.NotificationService.Domain.Templates;
using CC.NotificationService.Feature.Templates.Mapper;
using Microsoft.AspNetCore.Mvc;
using Wolverine.Http;
using Wolverine.Persistence;

namespace CC.NotificationService.Feature.Templates.Creation;

public class CreateTemplateFeature
{
    [ProducesResponseType<TemplateDto>(201)]
    [WolverinePost("api/v1/templates")]
    public static (IResult, Insert<Template>) Handle(
        CreateTemplateRequest request)
    {
        var template = new Template()
        {
            Id = Guid.CreateVersion7(),
            Key =  request.Key,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        return (Results.Created(), new Insert<Template>(template));
    }
}

public sealed record CreateTemplateRequest(string Key, bool IsActive);