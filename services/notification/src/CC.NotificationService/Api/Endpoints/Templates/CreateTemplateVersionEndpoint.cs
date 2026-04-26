using CC.NotificationService.Domain;
using CC.NotificationService.Domain.Dto;
using CC.NotificationService.Domain.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CC.NotificationService.Api.Endpoints.TemplateVersions;

public class CreateTemplateVersionEndpoint
{
    public static async Task<IResult> Handle(
      [FromServices] ITemplateVersionRepository repository,
      [FromBody] TemplateVersionDto templateVersionDto)
    {
        var templateVersion = new Domain.TemplateVersion()
        {
            TemplateKey = templateVersionDto.TemplateKey,
            Id = Guid.NewGuid(),
            IsActive = templateVersionDto.IsActive,
            CreateAt = DateTime.UtcNow,
            Channels = templateVersionDto.Channels.Select(c => new ChannelContext
            {
                Channel = c.Channel,
            }).ToList()
        };
        var createdTemplateVersion = await repository.CreateAsync(templateVersion);
        return Results.Ok(createdTemplateVersion);
    }
}