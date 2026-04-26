using CC.Common.Models;

namespace CC.NotificationService.Domain.interfaces;

public interface ITemplateVersionRepository
{
    Task<TemplateVersion> CreateAsync(TemplateVersion templateVersion);
    Task<TemplateVersion> GetByIdAsync(Guid Id);
    Task<TemplateVersion> DeleteByIdAsync(Guid Id);
    Task SaveAsync();
    void Update(TemplateVersion templateVersion);
    Task<PagedResult<TemplateVersion>> GetAllAsync(bool isActive, int page, int pageSize);
}