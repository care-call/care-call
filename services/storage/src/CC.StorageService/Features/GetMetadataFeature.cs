using CC.StorageService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CC.StorageService.Features;

public static class GetMetadataFeature
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/storage/metadata", HandleAsync)
            .WithName("GetMetadata");
    }

    private static async Task<IResult> HandleAsync(
        Guid[] ids,
        Db db)
    {
        if (ids == null || ids.Count() == 0)
            return Results.BadRequest(new { error = "No file IDs provided" });

        var files = await db.FileMetadata
            .Where(f => ids.ToList().Contains(f.Id))
            .Select(f => new
            {
                f.Id,
                f.OriginalName,
                f.ContentType,
                f.Size,
                f.Status,
                f.ExpiresAt,
                f.EntityType,
                f.EntityId,
                f.CreatedAt
            })
            .ToListAsync();

        return Results.Ok(files);
    }
}