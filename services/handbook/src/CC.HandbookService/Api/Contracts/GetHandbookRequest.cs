using CC.Common.Models;
using CC.HandbookService.Application.Enums;

namespace CC.HandbookService.Api.Contracts;

public sealed record GetHandbookRequest
{
    public required string Handbook { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public string? SearchName { get; init; }
    public string? SearchValue { get; init; }
    public string? SortBy { get; init; }
    public SortOrder? SortOrder { get; init; }
}