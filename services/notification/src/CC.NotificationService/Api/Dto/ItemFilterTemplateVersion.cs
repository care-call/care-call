using Microsoft.AspNetCore.Mvc;

namespace CC.NotificationService.Api.Dto;

public record ItemFilterTemplateVersion([FromQuery] bool IsActive);