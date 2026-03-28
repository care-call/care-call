using Vogen;

namespace CC.AppointmentService.Domain.Reviews.ValueObjects;

[ValueObject<int>]
public readonly partial struct ComfortRating
{
    private static Validation Validate(int value) =>
        value is >= 1 and <= 5
            ? Validation.Ok
            : Validation.Invalid("Comfort Rating");
}