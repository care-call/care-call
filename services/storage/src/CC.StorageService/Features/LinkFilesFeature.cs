using CC.StorageService.Entities.ValueObjects;
using CC.StorageService.Persistence;
using CC.StorageService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CC.StorageService.Features;

public static class LinkFilesFeature
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/storage/link", HandleAsync)
            .WithName("LinkFiles");
    }

    private static async Task<IResult> HandleAsync(
        LinkFilesRequest request,
        Db db)
    {
        foreach (var fileId in request.FileIds)
        {
            var metadata = await db.FileMetadata.FirstOrDefaultAsync(x => x.Id == fileId);
            if (metadata != null && metadata.Status == FileStatus.Temporary.Value)
            {
                metadata.Link(request.EntityType, request.EntityId, request.FieldName);
            }
        }

        await db.SaveChangesAsync();
        return Results.Ok();
    }

    public record LinkFilesRequest(
        List<Guid> FileIds,
        string EntityType,
        Guid EntityId,
        string FieldName
    );
}