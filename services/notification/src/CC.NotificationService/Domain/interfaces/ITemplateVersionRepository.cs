namespace CC.NotificationService.Domain.Interfaces;

public interface ITemplateVersionRepository
{
    ValueTask AddAsync(TemplateVersion templateVersion);
    Task<TemplateVersion> GetByIdAsync(Guid Id);
    Task DeleteByIdAsync(TemplateVersion templateVersion);
}