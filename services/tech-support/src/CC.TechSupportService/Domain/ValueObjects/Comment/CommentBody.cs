using Vogen;

namespace CC.TechSupportService.Domain.ValueObjects.Comment;

[ValueObject(typeof(string))]
public partial record CommentBody
{
    public const int MaxBodyLenght = 1024;
    
    private static Validation Validate(string body)
    {
        if (string.IsNullOrEmpty(body))
            return Validation.Invalid("Body cannot be empty!");
        else if (body.Length > MaxBodyLenght)
            return Validation.Invalid($"Body length cannot be greater than {MaxBodyLenght}");
        
        return Validation.Ok;
    }
}