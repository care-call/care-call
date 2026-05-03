using CC.Common.Models;
using CC.NotificationService.Application.Dependencies;
using CC.NotificationService.Domain;
using CC.NotificationService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CC.NotificationService.Infrastructure.Services;

public class TemplateQueryService(DatabaseContext _databaseContext) : ITemplateQuery
{
    public async Task<PagedResult<Template>> GetAllAsync(string key, bool isActive, int page, int pageSize)
    {
        var query = _databaseContext.Templates
        .Where(t => t.IsActive == isActive && t.Key.StartsWith(key));

        var totalItems = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        return new PagedResult<Template>(items, new PageInfo(page, pageSize), totalItems, totalPages);
    }
}