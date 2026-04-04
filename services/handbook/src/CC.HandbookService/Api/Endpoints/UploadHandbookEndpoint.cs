using CC.HandbookService.Application.Dependencies;
using CC.HandbookService.Application.UseCases;
using Mediator;

namespace CC.HandbookService.Api.Endpoints;

public static class UploadHandbookEndpoint
{
    public static async Task<IResult> Handle(
        string handbook,
        HttpRequest request,
        IMediator mediator)
    {
        if (!Enum.TryParse<HandbookType>(handbook, true, out var handbookType))
            return Results.BadRequest($"Справочник «{handbook}» не существует");
        
        var form = await request.ReadFormAsync();
        var handbookFile = form.Files.GetFile("handbookFile");
        
        if (handbookFile is null)
            return Results.BadRequest("Файл не передан");
        
        if (!handbookFile.FileName.EndsWith(".csv"))
            return Results.BadRequest("Допускаются только csv файлы");
        
        await using var stream = handbookFile.OpenReadStream();
        
        var result = await mediator.Send(new UploadHandbook
        {
            HandbookType = handbookType,
            HandbookFileStream = stream
        });
        
        if (result.IsSuccess)
            return Results.Ok(result);
        
        return Results.BadRequest(result.Errors);
    }
}