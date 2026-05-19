using CC.StorageService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CC.StorageService.Features;

public static class DeleteFileFeature
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/files/{fileId:guid}", HandleAsync)
            .WithName("DeleteFile");
    }

    private static async Task<IResult> HandleAsync(Guid fileId, Db db)
    {
        var metadata = await db.FileMetadata.FirstOrDefaultAsync(f => f.Id == fileId);
        if (metadata is null)
            return Results.NotFound(new { error = $"Файл {fileId} не найден" });

        metadata.Delete();
        await db.SaveChangesAsync();

        return Results.NoContent();
    }
}