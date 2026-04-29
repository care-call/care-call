using CC.TechSupportService.Domain.Enums;
using Vogen;

namespace CC.TechSupportService.Domain.ValueObjects.FileDetails;

[ValueObject(typeof(string))]
public sealed partial class FileName
{
    public const int MaxFileNameLength = 128;
    
    private static Validation Validate(string value)
    {
        if (string.IsNullOrEmpty(value))
            return Validation.Invalid(TechServiceErrors.FileNameCannotBeEmpty.ToString());
        else if (value.Length > MaxFileNameLength)
            return Validation.Invalid(TechServiceErrors.FileNameLengthExceeded.ToString());

        return Validation.Ok;
    }
}