using FluentResults;

namespace CC.TechSupportService.Domain.ValueObjects.Ticket;

public record TicketDescription
{
    public const int MaxDescriptionLenght = 256;
    
    private TicketDescription(string description) => Description = description;

    public static Result<TicketDescription> TryCreate(string description)
    {
        if (string.IsNullOrEmpty(description))
            return Result.Fail("Description cannot be null or empty");
        else if (description.Length > MaxDescriptionLenght)
            return Result.Fail($"Description cannot be greater than {MaxDescriptionLenght}");
        
        return Result.Ok(new TicketDescription(description));
    }
    
    public string Description { get; private set; }
}