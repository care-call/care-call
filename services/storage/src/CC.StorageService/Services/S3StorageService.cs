using Amazon.S3;
using Amazon.S3.Model;
using CC.StorageService.Entities;
using CC.StorageService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CC.StorageService.Services;

public class S3StorageService(
    IAmazonS3 s3,
    Db db,
    IConfiguration config,
    ILogger<S3StorageService> logger) : IStorageService
{
    private readonly string _bucketName = config["S3_BUCKET"] ?? "care-call-storage";

    public async Task<Guid> UploadTemporaryAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        long fileSize,
        Guid userId)
    {
        // 1. Создаем entity с бизнес-логикой
        var fileId = Guid.NewGuid();
        var metadata = FileMetadata.CreateNew(fileName, contentType, fileSize, userId);

        // 2. Генерируем S3 ключ
        var s3Key = $"temp/{fileId}/{Guid.NewGuid()}_{fileName}";
        metadata.SetS3Key(s3Key);

        // 3. Загружаем в S3
        var putRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = s3Key,
            InputStream = fileStream,
            ContentType = contentType
        };

        await s3.PutObjectAsync(putRequest);

        // 4. Сохраняем в БД
        await db.AddAsync(metadata);
        await db.SaveChangesAsync();

        logger.LogInformation("Uploaded temporary file {FileId} by user {UserId}", fileId, userId);

        return fileId;
    }

    public async Task<(Stream Stream, string ContentType, string FileName)> DownloadAsync(Guid fileId)
    {
        var metadata = await db.FileMetadata.FirstOrDefaultAsync(x => x.Id == fileId);
        if (metadata == null)
            throw new FileNotFoundException($"File {fileId} not found");

        var getRequest = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = metadata.S3Key
        };

        var response = await s3.GetObjectAsync(getRequest);

        return (response.ResponseStream, metadata.ContentType, metadata.OriginalName);
    }

    public async Task LinkFilesAsync(List<Guid> fileIds, string entityType, Guid entityId, string fieldName)
    {
        foreach (var fileId in fileIds)
        {
            var metadata = await db.FileMetadata.FirstOrDefaultAsync(x => x.Id == fileId);
            if (metadata != null)
            {
                // Бизнес-логика внутри entity
                metadata.Link(entityType, entityId, fieldName);
                db.Update(metadata);
                await db.SaveChangesAsync();
            }
        }

        logger.LogInformation("Linked {Count} files to {EntityType}/{EntityId}", fileIds.Count, entityType, entityId);
    }
}