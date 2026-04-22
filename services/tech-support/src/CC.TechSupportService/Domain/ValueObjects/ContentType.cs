using System.Text.RegularExpressions;
using FluentResults;

namespace CC.TechSupportService.Domain.ValueObjects;

public record ContentType
{
    private ContentType(string type) => Type = type;
    
    public string Type { get; private set; }

    public static Result<ContentType> TryCreate(string contentType)
    {
        if (string.IsNullOrEmpty(contentType))
            return Result.Fail("Content Type cannot be empty!");
        else if (!Regex.IsMatch(contentType, @"^[a-zA-Z0-9!#$%^&*_=+{}\-.]+/[a-zA-Z0-9!#$%^&*_=+{}\-.]+(?:\+[a-zA-Z0-9!#$%^&*_=+{}\-.]+)?$"))
            return Result.Fail("Passed string should be a MIME type!");
        
        return Result.Ok(new ContentType(contentType));
    }
}