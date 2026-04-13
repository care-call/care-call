using CC.Common.Models;
using CC.HandbookService.Application.Dependencies;
using CC.HandbookService.Domain.Enums;
using CC.HandbookService.Domain.Handbooks;

namespace CC.HandbookService.Application.UseCases;

public sealed record GetHandbook
{   
    public required HandbookType HandbookType { get; init; }
    public PageInfo PageInfo { get; init; }
    public string? SearchName { get; init; }
    public string? SearchValue { get; init; }
    public string? SortBy { get; init; }
    public SortOrder? SortOrder { get; init; }
}

public class GetHandbookUseCase(IServiceProvider serviceProvider) 
{
    public async ValueTask<PagedResult<HandbookItem>> Handle(GetHandbook command, CancellationToken cancellationToken)
    {
        var queryService = serviceProvider.GetRequiredKeyedService<IHandbookQueryService>(command.HandbookType);

        return await queryService.GetPageAsync(
            command.PageInfo,
            command.SearchName,
            command.SearchValue,
            command.SortBy,
            command.SortOrder);
    }
}