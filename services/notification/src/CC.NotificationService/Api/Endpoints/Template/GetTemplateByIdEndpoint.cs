using CC.NotificationService.Domain.Interfaces;

namespace CC.NotificationService.Api.Endpoints.Template;

public class GetTemplateByIdEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        ITemplateRepository templateRepository)
    {
        var template = await templateRepository.GetByIdAsync(id);
        if (template == null)
            return Results.NotFound();
        
        return Results.Ok(template);
    }
}