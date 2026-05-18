using CC.StorageService.Entities.ValueObjects;
using CC.StorageService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CC.StorageService.Features;

public static class GetEntityFilesFeature
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/storage/entity/{entityType}/{entityId:guid}", HandleAsync)
            .WithName("GetEntityFiles")
            .DisableAntiforgery();
    }

    private static async Task<IResult> HandleAsync(
        string entityType,
        Guid entityId,
        Db db)
    {
        var files = await db.FileMetadata
            .Where(f => f.EntityType == entityType && f.EntityId == entityId && f.Status != FileStatus.Deleted.Value)
            .Select(f => new
            {
                f.Id,
                f.OriginalName,
                f.ContentType,
                f.Size,
                f.FieldName,
                f.CreatedAt
            })
            .ToListAsync();

        return Results.Ok(files);
    }
}