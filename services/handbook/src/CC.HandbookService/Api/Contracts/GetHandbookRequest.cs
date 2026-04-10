using CC.Common.Models;
using CC.HandbookService.Api.Binding;
using CC.HandbookService.Application.Enums;
using CC.HandbookService.Domain.Handbooks;
using Microsoft.AspNetCore.Mvc;

namespace CC.HandbookService.Api.Contracts;

public sealed record GetHandbookRequest
{
    [FromRoute] 
    public required HandbookTypeParameter HandbookTypeParameter { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public string? SearchName { get; init; }
    public string? SearchValue { get; init; }
    public string? SortBy { get; init; }
    public SortOrder? SortOrder { get; init; }
}