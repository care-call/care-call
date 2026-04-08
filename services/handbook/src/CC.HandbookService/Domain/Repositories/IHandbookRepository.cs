using CC.Common.Models;
using CC.HandbookService.Application.Enums;
using CC.HandbookService.Domain.Handbooks;

namespace CC.HandbookService.Domain.Repositories;

public interface IHandbookRepository<T>
{
    public Task<PagedResult<T>> GetPageAsync(
        PageInfo pageInfo,
        string? searchName,
        string? searchValue,
        string? sortBy,
        SortOrder? sortOrder);
    public Task DeleteAllAsync();
    public Task AddRangeAsync(IEnumerable<T> items);
}