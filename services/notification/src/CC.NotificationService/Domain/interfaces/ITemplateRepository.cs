namespace CC.NotificationService.Domain.Interfaces;

public interface ITemplateRepository
{
    ValueTask AddAsync(Template template);
    ValueTask<Template> GetByIdAsync(Guid templateVersionId);
}