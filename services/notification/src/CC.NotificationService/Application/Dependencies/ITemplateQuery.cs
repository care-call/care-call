using CC.Common.Models;
using CC.NotificationService.Domain;

namespace CC.NotificationService.Application.Dependencies;

public interface ITemplateQuery
{
    Task<PagedResult<Template>> GetAllAsync(string key, bool isActive, int page, int pageSize);
}