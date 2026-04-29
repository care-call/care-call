using CC.TechSupportService.Domain.Enums;
using Vogen;

namespace CC.TechSupportService.Domain.ValueObjects.Comment;

[ValueObject(typeof(string))]
public partial record CommentBody
{
    public const int MaxBodyLenght = 1024;
    public const int MinBodyLenght = 16;
    
    private static Validation Validate(string body)
    {
        if (string.IsNullOrEmpty(body))
            return Validation.Invalid(TechServiceErrors.EmptyCommentBody.ToString());
        if (body.Length > MaxBodyLenght)
            return Validation.Invalid(TechServiceErrors.CommentBodyLengthExceeded.ToString());
        if (body.All(x => x.Equals(' ')))
            return Validation.Invalid(TechServiceErrors.EmptyCommentBody.ToString());
        if (body.Length < 16)
            return Validation.Invalid(TechServiceErrors.BodyLengthTooShort.ToString());
        
        return Validation.Ok;
    }
}