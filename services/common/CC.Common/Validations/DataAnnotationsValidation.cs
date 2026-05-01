using System.ComponentModel.DataAnnotations;

namespace CC.Common.Validations;

public static class DataAnnotationsValidation
{
    public static Dictionary<string, string[]> ValidateByDataAnnotations<T>(T model)
        where T : notnull
    {
        object boxedModel = model;

        var results = new List<ValidationResult>();

        if (Validator.TryValidateObject(
                boxedModel,
                new ValidationContext(boxedModel),
                results,
                validateAllProperties: true))
        {
            return [];
        }

        return results
            .SelectMany(x => x.MemberNames.Select(member => new
            {
                member,
                error = x.ErrorMessage ?? "Validation error"
            }))
            .GroupBy(x => x.member)
            .ToDictionary(
                x => x.Key,
                x => x.Select(e => e.error).ToArray());
    }
}