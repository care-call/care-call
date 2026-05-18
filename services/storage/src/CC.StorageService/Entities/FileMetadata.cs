using CC.Shared.Domain;
using CC.Shared.Domain.Exceptions;
using CC.StorageService.Entities.ValueObjects;

namespace CC.StorageService.Entities;

public class FileMetadata : Entity<Guid>
{
    private FileMetadata(Guid id) : base(id) { }

    public static FileMetadata CreateNew(
        string originalName,
        string contentType,
        long size,
        Guid createdBy)
    {
        if (string.IsNullOrWhiteSpace(originalName))
            throw new DomainException("File name cannot be empty");

        if (size <= 0)
            throw new DomainException("File size must be positive");

        var metadata = new FileMetadata(Guid.NewGuid());
        metadata.OriginalName = originalName;
        metadata.ContentType = contentType;
        metadata.Size = size;
        metadata.CreatedBy = createdBy;
        metadata.CreatedAt = DateTime.UtcNow;
        metadata.SetTemporary();

        return metadata;
    }

    public string S3Key { get; private set; } = string.Empty;
    public string OriginalName { get; private set; } = string.Empty;
    public string ContentType { get; private set; } = string.Empty;
    public long Size { get; private set; }
    public string Status { get; private set; } = string.Empty;
    public Guid? EntityId { get; private set; }
    public string? EntityType { get; private set; }
    public string? FieldName { get; private set; }
    public Guid CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ExpiresAt { get; private set; }

    public FileStatus StatusValue => FileStatus.FromString(Status);

    public void SetS3Key(string s3Key)
    {
        if (string.IsNullOrWhiteSpace(s3Key))
            throw new DomainException("S3 key cannot be empty");
        S3Key = s3Key;
    }

    public void SetTemporary()
    {
        Status = "temporary";
        ExpiresAt = DateTime.UtcNow.AddHours(24);
        EntityId = null;
        EntityType = null;
        FieldName = null;
    }

    public void Link(string entityType, Guid entityId, string fieldName)
    {
        if (!StatusValue.CanBeLinked)
            throw new DomainException($"Cannot link file with status {Status}");

        if (string.IsNullOrWhiteSpace(entityType))
            throw new DomainException("Entity type cannot be empty");

        EntityType = entityType;
        EntityId = entityId;
        FieldName = fieldName;
        Status = "linked";
        ExpiresAt = null;
    }

    public void Delete()
    {
        if (!StatusValue.CanBeDeleted)
            throw new DomainException($"Cannot delete file with status {Status}");

        Status = "deleted";
    }

    public bool IsExpired()
    {
        return Status == "temporary" && ExpiresAt.HasValue && ExpiresAt.Value < DateTime.UtcNow;
    }

    public bool CanBeDownloaded() => StatusValue.CanBeDownloaded;
}