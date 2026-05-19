using CC.StorageService.Persistence;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CC.StorageService.Features;

public static class GetMetadataFeature
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/files/metadata", HandleAsync)
            .WithName("GetMetadata");
    }

    private static async Task<Result<List<FileMetadataItemResponse>>> HandleAsync(Guid[] ids, Db db)
    {
        var files = await db.FileMetadata
            .Where(f => ids.ToList().Contains(f.Id))
            .Select(f => new FileMetadataItemResponse(
                f.Id,
                f.OriginalName,
                f.ContentType,
                f.Size,
                f.Status,
                f.ExpiresAt,
                f.EntityType,
                f.EntityId,
                f.CreatedAt
            ))
            .ToListAsync();

        return Result.Ok(files);
    }
}

internal record FileMetadataItemResponse(Guid Id, string OriginalName, string ContentType, long Size, string Status, DateTime? ExpiresAt, string? EntityType, Guid? EntityId, DateTime CreatedAt);
