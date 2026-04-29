using CC.TechSupportService.Domain.Enums;
using Vogen;

namespace CC.TechSupportService.Domain.ValueObjects.Ticket;

/// <summary>
/// Оценка пользователя, от 1 до 5
/// </summary>
[ValueObject(typeof(byte))]
public partial record CsatRating
{
    private static Validation Validate(byte value) 
        => value > 5 ? Validation.Invalid(TechServiceErrors.CsatRatingExceedsMaxValue.ToString()) : Validation.Ok;
}