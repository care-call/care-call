using CC.Common.Models;

namespace CC.NotificationService.Domain.interfaces;

public interface ITemplateRepository
{
    Task<Template> CreateAsync(Template template);
    Task<Template> GetByIdAsync(Guid Id);
    Task<PagedResult<Template>> GetAllAsync(string key, bool isActive, int page, int pageSize);
    Task SaveAsync();
}