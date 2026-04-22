using FluentResults;

namespace CC.TechSupportService.Domain.ValueObjects.Ticket;

public record Rating
{
    private Rating(byte value) => CsatRating = value;
    
    /// <summary>
    /// Оценка пользователя, от 1 до 5
    /// </summary>
    public byte CsatRating { get; private set; }
    
    public static Result<Rating> TryCreate(byte value)
    {
        if (value > 5)
            return Result.Fail("Value cannot be greater than 5");
        
        return Result.Ok(new Rating(value));
    }
}