using Amazon.Runtime.Internal;
using Amazon.S3;
using Amazon.S3.Model;
using CC.StorageService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CC.StorageService.Features;

public static class DownloadFileFeature
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/files/{fileId:guid}", HandleAsync)
            .WithName("DownloadFile")
            .DisableAntiforgery();
    }

    private static async Task<IResult> HandleAsync(Guid fileId, IAmazonS3 s3, Db db, IConfiguration config)
    {
        var bucketName = config["S3:Bucket"] ?? "care-call-storage";

        var metadata = await db.FileMetadata.FirstOrDefaultAsync(x => x.Id == fileId);
        if (metadata is null)
            return Results.NotFound(new ErrorResponse { Message = $"Файл {fileId} не найден" });

        var getRequest = new GetObjectRequest
        {
            BucketName = bucketName,
            Key = metadata.S3Key
        };

        var response = await s3.GetObjectAsync(getRequest);

        return Results.File(response.ResponseStream, metadata.ContentType, metadata.OriginalName);
    }
}