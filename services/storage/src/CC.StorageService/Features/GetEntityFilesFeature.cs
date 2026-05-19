using CC.StorageService.Entities.ValueObjects;
using CC.StorageService.Persistence;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CC.StorageService.Features;

public static class GetEntityFilesFeature
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/files/entity/{entityType}/{entityId:guid}", HandleAsync)
            .WithName("GetEntityFiles")
            .DisableAntiforgery();
    }

    private static async Task<Result<List<EntityFileItemResponse>>> HandleAsync(string entityType, Guid entityId, Db db)
    {
        var files = await db.FileMetadata
            .Where(f => f.EntityType == entityType && f.EntityId == entityId && f.Status != FileStatus.Deleted.Value)
            .Select(f => new EntityFileItemResponse(
                f.Id,
                f.OriginalName,
                f.ContentType,
                f.Size,
                f.FieldName,
                f.CreatedAt
            ))
            .ToListAsync();

        return Result.Ok(files);
    }
}

internal record EntityFileItemResponse(Guid Id, string OriginalName, string ContentType, long Size, string? FieldName, DateTime CreatedAt);
