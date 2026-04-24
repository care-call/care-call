using FluentResults;

namespace CC.TechSupportService.Domain.ValueObjects.Ticket;

public record CsatComment
{
    private const int MaxCommentLength = 1024;
    
    private CsatComment(string comment) => Comment = comment;
    
    /// <summary>
    /// Комментарий по оценке
    /// </summary>
    public string Comment { get; private set; }
    
    public static Result<CsatComment> TryCreate(string comment)
    {
        if(comment == string.Empty)
            return Result.Fail("Comment cannot be empty");
        else if(comment.Length > MaxCommentLength)
            return Result.Fail($"Comment length cannot be greater than {MaxCommentLength}");
        
        return Result.Ok(new CsatComment(comment));
    }
}