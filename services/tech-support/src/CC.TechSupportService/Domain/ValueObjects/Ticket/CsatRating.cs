using FluentResults;

namespace CC.TechSupportService.Domain.ValueObjects.Ticket;

public record CsatRating
{
    private CsatRating(byte value) => Rating = value;
    
    /// <summary>
    /// Оценка пользователя, от 1 до 5
    /// </summary>
    public byte Rating { get; private set; }
    
    public static Result<CsatRating> TryCreate(byte value)
    {
        if (value > 5)
            return Result.Fail("Value cannot be greater than 5");
        
        return Result.Ok(new CsatRating(value));
    }
}