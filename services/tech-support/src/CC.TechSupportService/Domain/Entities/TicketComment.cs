using CC.Shared.Domain;
using CC.TechSupportService.Domain.Enums;
using CC.TechSupportService.Domain.ValueObjects;
using CC.TechSupportService.Domain.ValueObjects.Comment;
using FluentResults;

namespace CC.TechSupportService.Domain.Entities;

public sealed class TicketComment : Entity<Guid>
{
    private TicketComment(Guid id) : base(id) { }

    private TicketComment(Guid id, 
        Guid ticketId, 
        Author author, 
        CommentBody body, 
        bool isInternal, 
        DateTime createdAt) : base(id)
    {
        Id = id;
        TicketId = ticketId;
        Author = author;
        Body = body;
        IsInternal = isInternal;
        CreatedAt = createdAt;
    }

    public Guid TicketId { get; private set; }
    public Author Author { get; private set; }
    public CommentBody Body { get; private set; }
    public bool IsInternal { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static Result<TicketComment> TryCreate(Guid id,
        Guid ticketId, 
        Author author, 
        CommentBody body, 
        bool isInternal, 
        DateTime createdAt)
    {
        if (id.Equals(ticketId))
            return Result.Fail(TechServiceErrors.PassingTheSameTicketIdAsCommentId);
        
        return Result.Ok(new TicketComment(id, ticketId, author, body, isInternal, createdAt));
    }
}