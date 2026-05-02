using Microsoft.AspNetCore.Mvc;

namespace CC.NotificationService.Api.Dto;

public record ItemFilterTemplate([FromQuery] bool IsActive, [FromQuery] string Key);