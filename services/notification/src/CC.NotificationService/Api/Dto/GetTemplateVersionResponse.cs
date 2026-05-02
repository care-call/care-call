using CC.NotificationService.Domain;

namespace CC.NotificationService.Api.Dto;

public sealed record GetTemplateVersionResponse(Guid Id, string TemplateKey, int Version, bool IsActive,  IReadOnlyList<ChannelContext> Channels, DateTime CreatedAt);