using CC.Shared.Domain;
using CC.TechSupportService.Domain.ValueObjects;
using FluentResults;

namespace CC.TechSupportService.Domain.Entities;

public class FileDetails : Entity<GuidId>
{
    public const int MaxFileNameLength = 128;
    public const int MaxFilePathLength = 128;
    
    private FileDetails(GuidId id) : base(id) { }
    
    private FileDetails(GuidId id, GuidId attachmentId, string fileName, string filePath, ContentType contentType, long contentSize) : base(id)
    {
        FileName = fileName;
        FilePath = filePath;
        ContentType = contentType;
        ContentSize = contentSize;
    }

    public GuidId AttachmentId { get; private set; }
    
    public string FileName { get; private set; }
    
    public string FilePath { get; private set; }
    
    public ContentType ContentType { get; private set; }
    
    public long ContentSize { get; private set; }
    
    public static Result<FileDetails> TryCreate(GuidId id, GuidId attachmentId, string fileName, string filePath, ContentType contentType, long contentSize)
    {
        var errors = new List<string>();
        
        if(string.IsNullOrEmpty(fileName))
            errors.Add("File name cannot be empty");
        else if(fileName.Length > MaxFileNameLength)
            errors.Add($"File name length cannot be greater than {MaxFileNameLength}");
        else if(string.IsNullOrEmpty(filePath))
            errors.Add("File path cannot be empty");
        else if(filePath.Length > MaxFilePathLength)
            errors.Add($"File path length cannot be greater than {MaxFilePathLength}");
        else if(contentSize < 0)
            errors.Add("Content size cannot be less than 0");

        if (errors.Count is not 0)
            return Result.Fail(errors);
        
        return Result.Ok(new FileDetails(id, attachmentId, fileName, filePath, contentType, contentSize));
    }
}