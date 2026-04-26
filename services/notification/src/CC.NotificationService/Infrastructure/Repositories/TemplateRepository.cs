using CC.Common.Models;
using CC.NotificationService.Domain;
using CC.NotificationService.Domain.interfaces;
using CC.NotificationService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CC.NotificationService.Infrastructure.Repositories;

public class TemplateRepository : ITemplateRepository
{
    private readonly DatabaseContext _databaseContext;
    
    public TemplateRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Template> CreateAsync(Template template)
    {
        await _databaseContext.AddAsync(template);
        await _databaseContext.SaveChangesAsync();
        return template;
    }

    public async Task<PagedResult<Template>> GetAllAsync(string key, bool isActive, int page, int pageSize)
    {
        var query = _databaseContext.Templates
        .Where(x => x.IsActive == isActive && x.Key.StartsWith(key));

        var totalItems = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        return new PagedResult<Template>(items, new PageInfo(page, pageSize), totalItems, totalPages);
    }  
    
    public async Task<Template> GetByIdAsync(Guid Id)
    {
       var product = await _databaseContext.Templates.FindAsync(Id);
       return product;
    }

    public async Task SaveAsync()
    {
      await _databaseContext.SaveChangesAsync();
    }
}