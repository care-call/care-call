using CC.Shared.Domain;
using CC.TechSupportService.Domain.Enums;
using FluentResults;

namespace CC.TechSupportService.Domain.Entities;

public sealed class TicketAttachment : Entity<Guid>
{
    private TicketAttachment(Guid id) : base(id) { }
    
    private TicketAttachment(Guid id, Guid ticketId, List<FileDetails> fileDetails, DateTime createdAt) : base(id)
    {
        TicketId = ticketId;
        _fileDefails = fileDetails;
        CreatedAt = createdAt;
    }

    public static Result<TicketAttachment> TryCreate(Guid id, Guid ticketId, List<FileDetails> fileDetails, DateTime createdAt)
    {
        if (id.Equals(ticketId))
            return Result.Fail(TechServiceErrors.PassingTheSameAttachmentIdAsTicketId);
        
        return Result.Ok(new TicketAttachment(id, ticketId, fileDetails, createdAt));
    }
    
    public Guid TicketId { get; private set; }

    public IReadOnlyList<FileDetails> FileDetails => _fileDefails;

    private List<FileDetails> _fileDefails;
    
    public DateTime CreatedAt { get; private set; }
}