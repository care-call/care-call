namespace CC.NotificationService.Api.Dto;

public sealed record GetTemplateResponse(Guid Id, string Key, bool IsActive);