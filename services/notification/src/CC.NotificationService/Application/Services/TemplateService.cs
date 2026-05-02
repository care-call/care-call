using CC.Common.Models;
using CC.NotificationService.Api.Dto;
using CC.NotificationService.Application.Dependencies;
using CC.NotificationService.Application.Dependencies.UnitOfWork;
using CC.NotificationService.Domain;
using CC.NotificationService.Domain.Interfaces;
using FluentResults;

namespace CC.NotificationService.Application.Services;

public class TemplateService(ITemplateRepository _templateRepository, ITemplateQuery _templateQueryService, IUnitOfWork _unitOfWork) : ITemplateService
{
    public async Task<Result<Template>> CreateAsync(CreateTemplateRequest request)
    {
        var template = new Template
        {
            Id = Guid.NewGuid(),
            IsActive = request.IsActive,
            Key = request.Key,
        };

        await _templateRepository.AddAsync(template);
        await _unitOfWork.SaveAsync();

        return Result.Ok(template);
    }

    public async Task<Result<PagedResult<Template>>> GetAllAsync(string key, bool isActive, int pageNumber, int pageSize)
    {
        var templates = await _templateQueryService.GetAllAsync(key, isActive, pageNumber, pageSize);

        if (templates is null)
            return Result.Fail("Шаблоны не найдены");

        return Result.Ok(templates);
    }
}