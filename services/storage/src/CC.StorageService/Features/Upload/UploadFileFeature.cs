using CC.StorageService.Services;

namespace CC.StorageService.Features.Upload;

public static class UploadFileFeature
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/storage/upload", HandleAsync)
          .WithName("UploadFile")
          .DisableAntiforgery();
    }

    private static async Task<IResult> HandleAsync(
        IFormFile file,
        IStorageService storageService,
        HttpContext httpContext)
    {
        if (file == null || file.Length == 0)
            return Results.BadRequest(new { error = "No file provided" });

        if (file.Length > 100 * 1024 * 1024)
            return Results.BadRequest(new { error = "File too large (max 100 MB)" });

        var userId = GetUserId(httpContext);

        // Вызываем сервис
        var fileId = await storageService.UploadTemporaryAsync(
            file.OpenReadStream(),
            file.FileName,
            file.ContentType,
            file.Length,
            userId);

        return Results.Ok(new { fileId });
    }

    private static Guid GetUserId(HttpContext httpContext)
    {
        // TODO: взять из JWT токена или заголовка
        return Guid.NewGuid(); // временно для демо
    }
}