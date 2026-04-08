using CC.Common.Models;
using CC.HandbookService.Application.Enums;
using CC.HandbookService.Domain.Handbooks;

namespace CC.HandbookService.Application.Dependencies;

public interface IHandbookQueryService
{
    public Task<PagedResult<HandbookItem>> GetPageAsync(
        PageInfo pageInfo,
        string? searchName,
        string? searchValue,
        string? sortBy,
        SortOrder? sortOrder);
}