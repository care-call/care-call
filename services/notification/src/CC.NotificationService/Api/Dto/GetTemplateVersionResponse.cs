using CC.NotificationService.Domain;

namespace CC.NotificationService.Api.Dto;

public record GetTemplateVersionResponse(Guid Id, string TemplateKey, int Version, bool IsActive, List<ChannelContext> Channels, DateTime CreatedAt);