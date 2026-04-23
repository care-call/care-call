using FluentResults;

namespace CC.TechSupportService.Domain.ValueObjects.Comment;

public record CommentBody
{
    public const int MaxBodyLenght = 1024;
    
    private CommentBody() {  }
    
    private CommentBody(string body) => Body = body;
    
    public string Body { get; private set; }

    public static Result<CommentBody> TryCreate(string body)
    {
        if (string.IsNullOrEmpty(body))
            return Result.Fail("Body cannot be empty!");
        else if (body.Length > MaxBodyLenght)
            return Result.Fail($"Body length cannot be greater than {MaxBodyLenght}");
        
        return Result.Ok();
    }
}