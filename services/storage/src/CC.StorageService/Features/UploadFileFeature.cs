using Amazon.S3;
using Amazon.S3.Model;
using CC.StorageService.Entities;
using CC.StorageService.Persistence;

namespace CC.StorageService.Features;

public static class UploadFileFeature
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/files/upload", HandleAsync)
          .WithName("UploadFile")
          .DisableAntiforgery();
    }

    private static async Task<IResult> HandleAsync(IFormFile file, IAmazonS3 s3, Db db, HttpContext httpContext, IConfiguration config)
    {
        var bucketName = config["S3:Bucket"] ?? "care-call-storage";
        var userId = GetUserId(httpContext);

        var fileId = Guid.CreateVersion7();
        var metadata = FileMetadata.CreateNew(file.FileName, file.ContentType, file.Length, userId);

        var s3Key = $"temp/{fileId}/{Guid.CreateVersion7()}_{file.FileName}";
        var fileStream = file.OpenReadStream();

        var putRequest = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = s3Key,
            InputStream = fileStream,
            ContentType = file.ContentType,
        };

        await s3.PutObjectAsync(putRequest);

        await db.AddAsync(metadata);
        await db.SaveChangesAsync();

        return Results.Ok(new FileIdResponse(fileId));
    }

    private static Guid GetUserId(HttpContext httpContext)
    {
        return Guid.CreateVersion7();
    }
}

internal record FileIdResponse(Guid FileId);
