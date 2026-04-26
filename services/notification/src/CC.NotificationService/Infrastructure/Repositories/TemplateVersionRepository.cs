using CC.Common.Models;
using CC.NotificationService.Domain;
using CC.NotificationService.Domain.interfaces;
using CC.NotificationService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CC.NotificationService.Infrastructure.Repositories;

public class TemplateVersionRepository : ITemplateVersionRepository
{
    private readonly DatabaseContext _databaseContext;
    public TemplateVersionRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<TemplateVersion> CreateAsync(TemplateVersion templateVersion)
    {
        await _databaseContext.AddAsync(templateVersion);
        await _databaseContext.SaveChangesAsync();
        return templateVersion;
    }

    public async Task<TemplateVersion> DeleteByIdAsync(Guid Id)
    {
        var product = await _databaseContext.TemplateVersions.FindAsync(Id);
        
        if(product == null)
        {
            return null;
        }
        
        _databaseContext.TemplateVersions.Remove(product);
        
        await _databaseContext.SaveChangesAsync();
        
        return product;
    }

    public async Task<PagedResult<TemplateVersion>> GetAllAsync(bool isActive, int page, int pageSize)
    {
        var query = _databaseContext.TemplateVersions
        .Where(x => x.IsActive == isActive);

        var totalItems = await query.CountAsync();

        var items = await query
            .OrderByDescending(x => x.CreateAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        return new PagedResult<TemplateVersion>(items, new PageInfo(page,pageSize), totalItems, totalPages);        
    }   
   
    public async Task<TemplateVersion> GetByIdAsync(Guid Id)
    {
       var product = await _databaseContext.TemplateVersions.FindAsync(Id);
       return product;
    }

    public async Task SaveAsync()
    {
      await _databaseContext.SaveChangesAsync();
    }

    public void Update(TemplateVersion templateVersion)
    {
        _databaseContext.Update(templateVersion);
    }
}