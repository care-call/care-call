using CC.NotificationService.Domain;
using CC.NotificationService.Domain.Interfaces;
using CC.NotificationService.Infrastructure.Persistence;

namespace CC.NotificationService.Infrastructure.Repositories;

public class TemplateVersionRepository(DatabaseContext _databaseContext) : ITemplateVersionRepository
{
    public async ValueTask AddAsync(TemplateVersion templateVersion)
        => await _databaseContext.AddAsync(templateVersion);

    public Task DeleteByIdAsync(TemplateVersion templateVersion)
    {
          _databaseContext.TemplateVersions.Remove(templateVersion);     
          return Task.CompletedTask;
    }
       
    public async Task<TemplateVersion> GetByIdAsync(Guid Id)
        => await _databaseContext.TemplateVersions.FindAsync(Id);
}