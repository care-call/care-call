using Vogen;

namespace CC.TechSupportService.Domain.ValueObjects.Ticket;

[ValueObject(typeof(string))]
public partial record TicketDescription
{
    public const int MaxDescriptionLenght = 256;
    
    private static Validation Validate(string description)
    {
        if (string.IsNullOrEmpty(description))
            return Validation.Invalid("Description cannot be null or empty");
        else if (description.Length > MaxDescriptionLenght)
            return Validation.Invalid($"Description cannot be greater than {MaxDescriptionLenght}");
        
        return Validation.Ok;
    }
}