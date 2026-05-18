using CC.StorageService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CC.StorageService.Features;

public static class DeleteFileFeature
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/storage/{fileId:guid}", HandleAsync)
            .WithName("DeleteFile");
    }

    private static async Task<IResult> HandleAsync(
        Guid fileId,
        Db db)
    {
        var metadata = await db.FileMetadata.FirstOrDefaultAsync(f => f.Id == fileId);
        if (metadata == null)
            return Results.NotFound(new { error = $"File {fileId} not found" });

        metadata.Delete();
        await db.SaveChangesAsync();

        return Results.NoContent();
    }
}