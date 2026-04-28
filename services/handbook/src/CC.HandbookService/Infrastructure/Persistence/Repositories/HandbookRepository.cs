using CC.Common.Models;
using CC.HandbookService.Domain.Enums;
using CC.HandbookService.Domain.Handbooks;
using CC.HandbookService.Domain.Repositories;
using CC.HandbookService.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CC.HandbookService.Infrastructure.Persistence.Repositories;

public class HandbookRepository<T>(DatabaseContext context): IHandbookRepository<T> where T : HandbookItem
{
    public Task<PagedResult<T>> GetPageAsync(
        PageInfo pageInfo, 
        string? searchName, 
        string? searchValue, 
        string? sortBy, 
        SortOrder? sortOrder)
    {
        IQueryable<T> query = context.Set<T>();
        
        if (searchName != null && searchValue != null)
            query = query.ApplySearch(searchName, searchValue);
    
        if (sortBy != null && sortOrder != null)
            query = query.ApplySort(sortBy, sortOrder.Value);


        return query.ToPagedResultAsync(pageInfo);
    }

    public Task DeleteAllAsync() => context.Set<T>().ExecuteDeleteAsync();

    public Task AddRangeAsync(IEnumerable<T> items)
    {
        context.Set<T>().AddRange(items);
        return Task.CompletedTask;
    }
}