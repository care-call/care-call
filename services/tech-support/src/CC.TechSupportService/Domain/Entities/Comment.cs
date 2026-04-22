using CC.Shared.Domain;
using CC.TechSupportService.Domain.ValueObjects;
using CC.TechSupportService.Domain.ValueObjects.Comment;
using FluentResults;

namespace CC.TechSupportService.Domain.Entities;

public class Comment : Entity<GuidId>
{
    private Comment(GuidId id) : base(id) { }

    private Comment(GuidId id, 
        GuidId ticketId, 
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

    public GuidId TicketId { get; private set; }

    public Author Author { get; private set; }

    public CommentBody Body { get; private set; }

    public bool IsInternal { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public static Result<Comment> TryCreate(GuidId id,
        GuidId ticketId, 
        Author author, 
        CommentBody body, 
        bool isInternal, 
        DateTime createdAt)
    {
        if (id.Equals(ticketId))
            return Result.Fail("Ticket Id cannot be the same as Comment Id");
        
        return Result.Ok(new Comment(id, ticketId, author, body, isInternal, createdAt));
    }
}