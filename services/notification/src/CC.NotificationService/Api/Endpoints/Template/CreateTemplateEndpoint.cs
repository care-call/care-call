using CC.NotificationService.Api.Dto;
using CC.NotificationService.Application.Dependencies;

namespace CC.NotificationService.Api.Endpoints.Template;

public class CreateTemplateEndpoint
{
    public static async Task<IResult> Handle(
        ITemplateService templateService,
        CreateTemplateRequest request)
    {   
        var result = await templateService.CreateAsync(request);

        if (!result.IsSuccess)
            return Results.BadRequest(result);
        
        var value = result.Value;
        var response = new GetTemplateResponse(value.Id, value.Key, value.IsActive);
            
        return Results.Ok(response);
    }
}