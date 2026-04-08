using CC.Common.Models;
using CC.HandbookService.Application.Dependencies;
using CC.HandbookService.Application.Enums;
using CC.HandbookService.Domain.Handbooks;
using CC.HandbookService.Domain.Repositories;
using FluentResults;
using Mediator;

namespace CC.HandbookService.Application.UseCases;

public sealed record GetHandbook : IRequest<PagedResult<HandbookItem>>
{
    public required HandbookType HandbookType { get; init; }
    public PageInfo PageInfo { get; init; }
    public string? SearchName { get; init; }
    public string? SearchValue { get; init; }
    public string? SortBy { get; init; }
    public SortOrder? SortOrder { get; init; }
}

public class GetHandbookUseCase(
    IServiceProvider serviceProvider) : IRequestHandler<GetHandbook, PagedResult<HandbookItem>>
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