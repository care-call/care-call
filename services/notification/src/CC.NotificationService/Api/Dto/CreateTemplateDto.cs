namespace CC.NotificationService.Api.Dto;

public sealed record CreateTemplateDto(Guid Id, string Key, bool IsActive, DateTime CreatedAt);