using Vogen;

namespace CC.TechSupportService.Domain.ValueObjects.Ticket;

/// <summary>
/// Комментарий по оценке
/// </summary>
[ValueObject(typeof(string))]
public partial record CsatComment
{
    private const int MaxCommentLength = 1024;
    
    private static Validation Validate(string comment)
    {
        if(comment == string.Empty)
            return Validation.Invalid("Comment cannot be empty");
        else if(comment.Length > MaxCommentLength)
            return Validation.Invalid($"Comment length cannot be greater than {MaxCommentLength}");
        
        return Validation.Ok;
    }
}