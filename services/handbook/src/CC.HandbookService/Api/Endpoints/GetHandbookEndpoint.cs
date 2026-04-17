using CC.Common.Models;
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
            
        var result = await bus.InvokeAsync<PagedResult<HandbookItem>>(new GetHandbook
        {
            HandbookType = request.HandbookTypeParameter.HandbookType,
            PageInfo = new PageInfo(request.PageNumber, request.PageSize),
            SearchName = request.SearchName,
            SearchValue = request.SearchValue,
            SortBy = request.SortBy,
            SortOrder = request.SortOrder
        });
        
        return Results.Ok(result);
    }
}