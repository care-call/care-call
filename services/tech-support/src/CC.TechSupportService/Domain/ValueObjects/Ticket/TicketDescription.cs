using CC.TechSupportService.Domain.Enums;
using Vogen;

namespace CC.TechSupportService.Domain.ValueObjects.Ticket;

[ValueObject(typeof(string))]
public partial record TicketDescription
{
    public const int MaxDescriptionLenght = 256;
    
    private static Validation Validate(string description)
    {
        if (string.IsNullOrEmpty(description))
            return Validation.Invalid(TechServiceErrors.TicketDescriptionIsEmpty.ToString());
        else if (description.Length > MaxDescriptionLenght)
            return Validation.Invalid(TechServiceErrors.TicketDescriptionIsTooLong.ToString());
        
        return Validation.Ok;
    }
}