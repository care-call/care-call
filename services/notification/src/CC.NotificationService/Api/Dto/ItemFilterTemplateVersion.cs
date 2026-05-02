using Microsoft.AspNetCore.Mvc;

namespace CC.NotificationService.Api.Dto;

public sealed record ItemFilterTemplateVersion([FromQuery] bool IsActive);