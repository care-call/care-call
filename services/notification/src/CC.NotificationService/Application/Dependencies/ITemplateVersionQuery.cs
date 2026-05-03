using CC.Common.Models;
using CC.NotificationService.Domain;

namespace CC.NotificationService.Application.Dependencies;

public interface ITemplateVersionQuery
{
    Task<PagedResult<TemplateVersion>> GetAllAsync(bool isActive, int page, int pageSize);
}