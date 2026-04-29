using System.Text.RegularExpressions;
using CC.TechSupportService.Domain.Enums;
using Vogen;

namespace CC.TechSupportService.Domain.ValueObjects.FileDetails;

[ValueObject(typeof(string))]
public partial record ContentType
{
    public const int MaxContentTypeLength = 256;
    
    [GeneratedRegex(@"^[a-zA-Z0-9!#$%^&*_=+{}\-.]+/[a-zA-Z0-9!#$%^&*_=+{}\-.]+(?:\+[a-zA-Z0-9!#$%^&*_=+{}\-.]+)?$")]
    private static partial Regex ContentTypeRegex();
    
    private static Validation Validate(string contentType)
    {
        if (string.IsNullOrEmpty(contentType))
            return Validation.Invalid(TechServiceErrors.ContentTypeIsEmpty.ToString());
        else if (ContentTypeRegex().IsMatch(contentType))
            return Validation.Invalid(TechServiceErrors.ContentTypeIsNotMimeType.ToString());
        else if(contentType.Length > MaxContentTypeLength)
            return Validation.Invalid(TechServiceErrors.ContentTypeIsTooLong.ToString());

        return Validation.Ok;
    }
}