namespace CC.NotificationService.Api.Dto;

public record CreateTemplateDto(Guid Id, string Key, bool IsActive, DateTime CreatedAt);