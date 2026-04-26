using CC.NotificationService.Domain.Dto;

namespace CC.NotificationService.Domain.interfaces;

public interface ITemplateVersionService
{
    Task<TemplateVersion> CreateAsync(TemplateVersionDto templateVersionDto);
    Task<TemplateVersionDto> GetByIdAsync(Guid Id);
    Task<TemplateVersionDto> GetAllAsync();
    void UpdateAsync(TemplateVersionDto templateVersionDto);
    Task<TemplateVersionDto> DeleteByIdAsync(Guid Id);
}