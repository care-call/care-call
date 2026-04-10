using CC.HandbookService.Api.Binding;
using CC.HandbookService.Api.Contracts;
using CC.HandbookService.Application.UseCases;
using CC.HandbookService.Domain.Errors;
using CC.HandbookService.Domain.Handbooks;
using Mediator;

namespace CC.HandbookService.Api.Endpoints;

public static class UploadHandbookEndpoint
{
    public static async Task<IResult> Handle(
        HandbookTypeParameter handbookTypeParameter,
        HttpRequest request,
        IMediator mediator)
    {
        if (handbookTypeParameter.Error is not null)
            return Results.BadRequest(handbookTypeParameter.Error);
        
        var form = await request.ReadFormAsync();
        var handbookFile = form.Files.GetFile("handbookFile");
        
        if (handbookFile is null)
            return Results.BadRequest("Файл не передан");
        
        if (!handbookFile.FileName.EndsWith(".csv"))
            return Results.BadRequest("Допускаются только csv файлы");
        
        await using var stream = handbookFile.OpenReadStream();
        
        var result = await mediator.Send(new UploadHandbook
        {
            HandbookType = handbookTypeParameter.HandbookType,
            HandbookFileStream = stream
        });
        
        if (result.IsSuccess)
            return Results.Ok(result);
        
        return Results.BadRequest(result.Errors);
    }
}