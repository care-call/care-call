using Vogen;

namespace CC.TechSupportService.Domain.ValueObjects.Ticket;

/// <summary>
/// Оценка пользователя, от 1 до 5
/// </summary>
[ValueObject(typeof(byte))]
public partial record CsatRating
{
    private static Validation Validate(byte value)
    {
        if (value > 5)
            return Validation.Invalid("Value cannot be greater than 5");
        
        return Validation.Ok;
    }
}