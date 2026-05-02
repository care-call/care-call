using CC.NotificationService.Api.Dto;
using CC.NotificationService.Application.Dependencies.UnitOfWork;
using CC.NotificationService.Domain.Interfaces;

namespace CC.NotificationService.Domain.TemplateVersions;

public class CreateTemplateVersionEndpoint
{
    public static async Task<IResult> Handle(
      ITemplateVersionRepository repository,
      CreateTemplateVersionRequest request,
      IUnitOfWork unitOfWork)
    {
        var templateVersion = new TemplateVersion()
        {
            TemplateKey = request.TemplateKey,
            Id = Guid.CreateVersion7(),
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
            Channels = request.Channels.Select(c => new ChannelContext
            {
                Channel = c.Channel,
                Content = c.Content
            }).ToList()
        };
              
        await repository.AddAsync(templateVersion);
        await unitOfWork.SaveAsync();

        var templateResponse = new GetTemplateVersionResponse(templateVersion.Id, templateVersion.TemplateKey, templateVersion.Version, templateVersion.IsActive, templateVersion.Channels, templateVersion.CreatedAt);

        return Results.Ok(templateResponse);
    }
}