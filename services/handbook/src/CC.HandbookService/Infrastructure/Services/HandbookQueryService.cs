using CC.Common.Models;
using CC.HandbookService.Application.Dependencies;
using CC.HandbookService.Domain.Enums;
using CC.HandbookService.Domain.Handbooks;
using CC.HandbookService.Domain.Repositories;

namespace CC.HandbookService.Infrastructure.Services;

public class HandbookQueryService<T>(
    IHandbookRepository<T> repository) : IHandbookQueryService where T : HandbookItem
{
    public async Task<PagedResult<HandbookItem>> GetPageAsync(
        PageInfo pageInfo, 
        string? searchName, 
        string? searchValue, 
        string? sortBy, 
        SortOrder? sortOrder)
    {
        var result = await repository.GetPageAsync(pageInfo, searchName, searchValue, sortBy, sortOrder);
        
        return new PagedResult<HandbookItem>(result.Items, result.Page, result.TotalRows, result.TotalPages);
    }
}