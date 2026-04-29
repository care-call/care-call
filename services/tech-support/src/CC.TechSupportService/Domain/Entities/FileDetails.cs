using CC.Shared.Domain;
using CC.TechSupportService.Domain.Enums;
using CC.TechSupportService.Domain.ValueObjects.FileDetails;
using FluentResults;

namespace CC.TechSupportService.Domain.Entities;

public sealed class FileDetails : Entity<Guid>
{
    private FileDetails(Guid id) : base(id) { }
    
    private FileDetails(Guid id, Guid attachmentId, FileName fileName, FilePath filePath, ContentType contentType, long contentSize) : base(id)
    {
        FileName = fileName;
        FilePath = filePath;
        ContentType = contentType;
        ContentSize = contentSize;
        AttachmentId = attachmentId;
    }

    public Guid AttachmentId { get; private set; }
    public FileName FileName { get; private set; }
    public FilePath FilePath { get; private set; }
    public ContentType ContentType { get; private set; }
    public long ContentSize { get; private set; }
    
    public static Result<FileDetails> TryCreate(Guid id, Guid attachmentId, FileName fileName, FilePath filePath, ContentType contentType, long contentSize)
    {
        var errors = new List<Error>();
        
        if (contentSize < 0)
            errors.Add(TechServiceErrors.PassingNegativeContentSize);
        if (id.Equals(attachmentId))
            errors.Add(TechServiceErrors.PassingTheSameFileDetailsIdAsAttachmentId);

        if (errors.Count is not 0)
            return Result.Fail(errors);
        
        return Result.Ok(new FileDetails(id, attachmentId, fileName, filePath, contentType, contentSize));
    }
}