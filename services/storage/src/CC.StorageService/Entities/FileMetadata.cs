using CC.Shared.Domain;
using CC.Shared.Domain.Exceptions;
using CC.StorageService.Entities.ValueObjects;

namespace CC.StorageService.Entities;

public class FileMetadata : Entity<Guid>
{
    private FileMetadata(
        Guid id,
        string originalName,
        string contentType,
        long size,
        Guid createdBy,
        string s3Key) : base(id)
    {
        OriginalName = originalName;
        ContentType = contentType;
        Size = size;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
        S3Key = s3Key;
        Status = FileStatus.Temporary.Value;
        ExpiresAt = DateTime.UtcNow.AddHours(24);
    }

    public static FileMetadata CreateNew(
        string originalName,
        string contentType,
        long size,
        Guid createdBy)
    {
        if (string.IsNullOrWhiteSpace(originalName))
            throw new DomainException("Имя файла не может быть пустым");

        if (size <= 0)
            throw new DomainException("Размер файла должен быть больше нуля");

        var id = Guid.CreateVersion7();
        var s3Key = $"temp/{id}/{Guid.CreateVersion7()}_{originalName}";

        return new FileMetadata(id, originalName, contentType, size, createdBy, s3Key);
    }

    public string S3Key { get; private set; }
    public string OriginalName { get; private set; }
    public string ContentType { get; private set; }
    public long Size { get; private set; }
    public string Status { get; private set; }
    public Guid? EntityId { get; private set; }
    public string? EntityType { get; private set; }
    public string? FieldName { get; private set; }
    public Guid CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ExpiresAt { get; private set; }

    public FileStatus StatusValue => FileStatus.FromString(Status);

    public void Link(string entityType, Guid entityId, string fieldName)
    {
        if (!StatusValue.CanBeLinked)
            throw new DomainException($"Невозможно привязать файл со статусом {Status}");

        if (string.IsNullOrWhiteSpace(entityType))
            throw new DomainException("Тип сущности не может быть пустым");

        EntityType = entityType;
        EntityId = entityId;
        FieldName = fieldName;
        Status = FileStatus.Linked.Value;
        ExpiresAt = null;
    }

    public void Delete()
    {
        if (!StatusValue.CanBeDeleted)
            throw new DomainException($"Невозможно удалить файл со статусом {Status}");

        Status = FileStatus.Deleted.Value;
    }
}