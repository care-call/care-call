using CC.Common.Models;
using CC.HandbookService.Api.Contracts;
using CC.HandbookService.Application.UseCases;
using CC.HandbookService.Domain.Handbooks;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace CC.HandbookService.Api.Endpoints;

public static class GetHandbookEndpoint
{
    public static async Task<IResult> Handle(
        [AsParameters] GetHandbookRequest request, 
        IMediator mediator)
    {
        if (!Enum.TryParse<HandbookType>(request.Handbook, true, out var handbookType))
            return Results.BadRequest($"Справочник «{request.Handbook}» не существует");
        
        var result = await mediator.Send(new GetHandbook
        {
            HandbookType = handbookType,
            PageInfo = new PageInfo(request.PageNumber, request.PageSize),
            SearchName = request.SearchName,
            SearchValue = request.SearchValue,
            SortBy = request.SortBy,
            SortOrder = request.SortOrder
        });
        
        return Results.Ok(result);
    }
}