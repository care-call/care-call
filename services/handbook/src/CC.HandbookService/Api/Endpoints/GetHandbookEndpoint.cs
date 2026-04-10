using CC.Common.Models;
using CC.HandbookService.Api.Contracts;
using CC.HandbookService.Application.UseCases;
using Mediator;


namespace CC.HandbookService.Api.Endpoints;

public static class GetHandbookEndpoint
{
    public static async Task<IResult> Handle(
        [AsParameters] GetHandbookRequest request, 
        IMediator mediator)
    {
        if (request.HandbookTypeParameter.Error is not null)
            return Results.BadRequest(request.HandbookTypeParameter.Error);
            
        var result = await mediator.Send(new GetHandbook
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