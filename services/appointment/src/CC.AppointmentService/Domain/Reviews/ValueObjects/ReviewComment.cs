using Vogen;

namespace CC.AppointmentService.Domain.Reviews.ValueObjects;

[ValueObject<string>]
public readonly partial struct ReviewComment
{
    private static Validation Validate(string value) =>
        value.Length <= 1000
            ? Validation.Ok
            : Validation.Invalid("Review Comment"); 
}