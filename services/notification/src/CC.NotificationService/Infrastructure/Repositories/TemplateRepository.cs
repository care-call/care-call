using CC.NotificationService.Domain;
using CC.NotificationService.Domain.Interfaces;
using CC.NotificationService.Infrastructure.Persistence;

namespace CC.NotificationService.Infrastructure.Repositories;

public class TemplateRepository(DatabaseContext _databaseContext) : ITemplateRepository
{   
    public async ValueTask AddAsync(Template template)   
        => await _databaseContext.AddAsync(template);
         
    public ValueTask<Template> GetByIdAsync(Guid templateVersionId)
        => _databaseContext.Templates.FindAsync(templateVersionId);
}