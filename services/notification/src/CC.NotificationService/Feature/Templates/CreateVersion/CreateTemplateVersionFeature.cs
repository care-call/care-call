using CC.NotificationService.Domain.Templates;
using Microsoft.AspNetCore.Mvc;
using Wolverine.Http;
using Wolverine.Persistence;

namespace CC.NotificationService.Feature.Templates.CreateVersion;

public class CreateTemplateVersionFeature
{
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [WolverinePost("api/v1/templates/{key}/versions")]
    public static (IResult, Insert<TemplateVersion>) Handle(
        string key, 
        CreateTemplateVersionRequest request)
    {
        var templateVersion = new TemplateVersion()
        {
            TemplateKey = key,
            Id = Guid.CreateVersion7(),
            IsActive = request.IsActive,
            Version = new Random().Next(1, int.MaxValue),
            CreatedAt = DateTime.UtcNow,
            Channels = request.Channels.Select(c => new ChannelContext
            {
                Channel = c.Channel,
                Content = c.Content
            }).ToList()
        };
        
        return (Results.NoContent(), new Insert<TemplateVersion>(templateVersion));
    }
}

public sealed record CreateTemplateVersionRequest(bool IsActive, List<ChannelDto> Channels);
public sealed record ChannelDto(ChannelType Channel, string Content);