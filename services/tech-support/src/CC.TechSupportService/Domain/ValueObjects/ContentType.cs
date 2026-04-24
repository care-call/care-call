using System.Text.RegularExpressions;
using FluentResults;
using Vogen;

namespace CC.TechSupportService.Domain.ValueObjects;

[ValueObject(typeof(string))]
public partial record ContentType
{
    public const int MaxContentTypeLength = 256;
    
    public static Result<ContentType> TryCreate(string contentType)
    {
        var errors = new List<string>();
        
        if (string.IsNullOrEmpty(contentType))
            errors.Add("Content Type cannot be empty!");
        else if (!Regex.IsMatch(contentType, @"^[a-zA-Z0-9!#$%^&*_=+{}\-.]+/[a-zA-Z0-9!#$%^&*_=+{}\-.]+(?:\+[a-zA-Z0-9!#$%^&*_=+{}\-.]+)?$"))
            errors.Add("Passed string should be a MIME type!");
        else if(contentType.Length > MaxContentTypeLength)
            errors.Add($"Content type lenght cannot be greater than {MaxContentTypeLength}");

        if (errors.Count is not 0)
            return Result.Fail(errors);
        
        return Result.Ok(From(contentType));
    }
}