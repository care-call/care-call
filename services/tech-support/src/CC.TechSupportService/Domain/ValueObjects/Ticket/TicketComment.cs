using FluentResults;

namespace CC.TechSupportService.Domain.ValueObjects.Ticket;

public record TicketComment
{
    private const int MaxCommentLength = 1024;
    
    private TicketComment(string comment) => CsatComment = comment;
    
    /// <summary>
    /// Комментарий по оценке
    /// </summary>
    public string CsatComment { get; private set; }
    
    public static Result<TicketComment> TryCreate(byte value, string comment)
    {
        if(comment == string.Empty)
            return Result.Fail("Comment cannot be empty");
        else if(comment.Length > MaxCommentLength)
            return Result.Fail($"Comment length cannot be greater than {MaxCommentLength}");
        
        return Result.Ok(new TicketComment(comment));
    }
}