using CC.NotificationService.Domain;

namespace CC.NotificationService.Api.Dto;

public record ChannelDto(ChannelType Channel, string Content);