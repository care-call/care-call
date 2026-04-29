using Vogen;

namespace CC.TechSupportService.Domain.ValueObjects.FileDetails;

[ValueObject(typeof(string))]
public sealed partial class FilePath
{
    public const int MaxFilePathLength = 128;
    
    private static Validation Validate(string value)
    {
        if (string.IsNullOrEmpty(value))
            return Validation.Invalid("File path cannot be empty"); 
        if (value.Length > MaxFilePathLength)
            return Validation.Invalid($"File path length cannot be greater than {MaxFilePathLength}");
        if (value.Split('/').Length < 3)
            return Validation.Invalid("Not valid file path");

        return Validation.Ok;
    }
}