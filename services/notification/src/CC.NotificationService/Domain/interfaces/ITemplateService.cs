using CC.NotificationService.Domain.Dto;

namespace CC.NotificationService.Domain.interfaces;

public interface ITemplateService
{
    Task<Template> CreateAsync(TemplateDto templateDto);
    Task<TemplateDto> GetByIdAsync(Guid Id);
    Task<TemplateDto> DeleteByIdAsync(Guid Id);
    Task<List<TemplateDto>> GetAllAsync();
    void UpdateAsync(TemplateDto templateDto);
}