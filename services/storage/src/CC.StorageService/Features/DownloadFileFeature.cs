using CC.StorageService.Services;

namespace CC.StorageService.Features;

public static class DownloadFileFeature
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/storage/{fileId:guid}", HandleAsync)
            .WithName("DownloadFile")
            .DisableAntiforgery();
    }

    private static async Task<IResult> HandleAsync(
        Guid fileId,
        IStorageService storageService)
    {
        try
        {
            var (stream, contentType, fileName) = await storageService.DownloadAsync(fileId);
            return Results.File(stream, contentType, fileName);
        }
        catch (FileNotFoundException)
        {
            return Results.NotFound(new { error = $"File {fileId} not found" });
        }
    }
}