using CC.Common.Models;
using CC.NotificationService.Api.Dto;
using CC.NotificationService.Domain;
using FluentResults;

namespace CC.NotificationService.Application.Dependencies;

public interface ITemplateService
{
    Task<Result<Template>> CreateAsync(CreateTemplateRequest request);
    Task<Result<PagedResult<Template>>> GetAllAsync(string key, bool isActive, int pageNumber, int pageSize);
}