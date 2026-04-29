using CC.TechSupportService.Domain.Enums;
using Vogen;

namespace CC.TechSupportService.Domain.ValueObjects.Ticket;

/// <summary>
/// Комментарий по оценке
/// </summary>
[ValueObject(typeof(string))]
public partial record CsatComment
{
    public const int MaxCommentLength = 1024;
    public const int MinCommentLength = 16; 
    
    private static Validation Validate(string comment)
    {
        if(string.IsNullOrEmpty(comment))
            return Validation.Invalid(TechServiceErrors.CsatCommentIsEmpty.ToString());
        if(comment.Length > MaxCommentLength)
            return Validation.Invalid(TechServiceErrors.CsatCommentLengthExceeded.ToString());
        if (comment.Length < MinCommentLength)
            return Validation.Invalid(TechServiceErrors.CsatCommentLengthTooShort.ToString());
        
        return Validation.Ok;
    }
}