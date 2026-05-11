namespace CC.StorageService.Services;

public interface IStorageService
{
    Task<Guid> UploadTemporaryAsync(Stream fileStream, string fileName, string contentType, long fileSize, Guid userId);
    Task<(Stream Stream, string ContentType, string FileName)> DownloadAsync(Guid fileId);
    Task LinkFilesAsync(List<Guid> fileIds, string entityType, Guid entityId, string fieldName);
}