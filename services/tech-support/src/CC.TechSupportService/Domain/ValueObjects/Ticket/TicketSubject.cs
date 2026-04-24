using Vogen;

namespace CC.TechSupportService.Domain.ValueObjects.Ticket;

[ValueObject(typeof(string))]
public partial class TicketSubject
{
    public const int MaxSubjectLenght = 256;
    
    private static Validation Validate(string description)
    {
        if (string.IsNullOrEmpty(description))
            return Validation.Invalid("Subject cannot be null or empty");
        else if (description.Length > MaxSubjectLenght)
            return Validation.Invalid($"Subject cannot be greater than {MaxSubjectLenght}");

        return Validation.Ok;
    }
}