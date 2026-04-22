using CC.Shared.Domain;
using CC.TechSupportService.Domain.ValueObjects;
using FluentResults;

namespace CC.TechSupportService.Domain.Entities;

public class Attachment : Entity<GuidId>
{
    private Attachment(GuidId id, GuidId ticketId, List<FileDetails> fileDetails, DateTime createdAt) : base(id)
    {
        TicketId = ticketId;
        _fileDefails = fileDetails;
        CreatedAt = createdAt;
    }

    public static Result<Attachment> TryCreate(GuidId id, GuidId ticketId, List<FileDetails> fileDetails, DateTime createdAt)
    {
        if (id.Equals(ticketId))
            return Result.Fail("Attachment id cannot be the same as ticket id");
        
        return Result.Ok(new Attachment(id, ticketId, fileDetails, createdAt));
    }
    
    public GuidId TicketId { get; private set; }

    public IReadOnlyList<FileDetails> FileDetails => _fileDefails;

    private List<FileDetails> _fileDefails;
    
    public DateTime CreatedAt { get; private set; }
}