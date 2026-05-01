using System.ComponentModel.DataAnnotations;

namespace CC.TechSupportService.Features.Tickets.Validation;

public static class DataAnnotationsValidationExtensions
{
    extension<T>(T model)
    {
        public IResult? ValidateByDataAnnotations()
        {
            object boxedModel = model!;

            var results = new List<ValidationResult>();

            if (Validator.TryValidateObject(
                    boxedModel,
                    new ValidationContext(boxedModel),
                    results,
                    validateAllProperties: true))
                return null;

            var errors = results
                .SelectMany(x => x.MemberNames.Select(member => new
                {
                    member,
                    error = x.ErrorMessage ?? "Validation error"
                }))
                .GroupBy(x => x.member)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(e => e.error).ToArray());

            return Results.ValidationProblem(errors);
        }
    }
}