namespace CC.NotificationService.Api.Dto;

public sealed record CreateTemplateVersionRequest(int Version, string TemplateKey, bool IsActive, List<ChannelDto> Channels);