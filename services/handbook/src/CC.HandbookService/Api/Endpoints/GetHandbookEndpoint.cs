using CC.Common.Models;
using CC.Common.Pagination;
using CC.HandbookService.Api.Contracts;
using CC.HandbookService.Application.UseCases;
using CC.HandbookService.Domain.Handbooks;
using Wolverine;

namespace CC.HandbookService.Api.Endpoints;

public static class GetHandbookEndpoint
{
    public static async Task<IResult> Handle(
        [AsParameters] GetHandbookRequest request, 
        IMessageBus bus)
    {
        if (request.HandbookTypeParameter.Error is not null)
            return Results.BadRequest(request.HandbookTypeParameter.Error);

        var pageInfo = new PageInfo(request.PageNumber, request.PageSize);
        if (pageInfo.IsInvalid(out var validationErrors))
            return Results.ValidationProblem(validationErrors);
            
        var result = await bus.InvokeAsync<PagedResult<HandbookItem>>(new GetHandbook
        {
            HandbookType = request.HandbookTypeParameter.HandbookType,
            PageInfo = pageInfo,
            SearchName = request.SearchName,
            SearchValue = request.SearchValue,
            SortBy = request.SortBy,
            SortOrder = request.SortOrder
        });
        
        return Results.Ok(result);
    }
}
