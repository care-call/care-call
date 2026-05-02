using CC.NotificationService.Domain;

namespace CC.NotificationService.Api.Dto;

public sealed record ChannelDto(ChannelType Channel, string Content);