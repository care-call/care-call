using CC.Common.Models;
using CC.NotificationService.Domain;
using Microsoft.EntityFrameworkCore;
using CC.NotificationService.Infrastructure.Persistence;
using CC.NotificationService.Application.Dependencies;

namespace CC.NotificationService.Infrastructure.Services;

public class TemplateVersionQueryService(DatabaseContext _databaseContext) : ITemplateVersionQuery
{
    public async Task<PagedResult<TemplateVersion>> GetAllAsync(bool isActive, int page, int pageSize)
    {
        var query = _databaseContext.TemplateVersions
        .Where(tv => tv.IsActive == isActive);

        var totalItems = await query.CountAsync();

        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        return new PagedResult<TemplateVersion>(items, new PageInfo(page, pageSize), totalItems, totalPages);
    }
}