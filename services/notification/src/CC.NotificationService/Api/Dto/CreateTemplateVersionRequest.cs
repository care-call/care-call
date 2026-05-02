namespace CC.NotificationService.Api.Dto;

public record CreateTemplateVersionRequest(int Version, string TemplateKey, bool IsActive, List<ChannelDto> Channels);