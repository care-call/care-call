using CC.TechSupportService.Domain.Enums;
using Vogen;

namespace CC.TechSupportService.Domain.ValueObjects.Ticket;

[ValueObject(typeof(string))]
public partial class TicketSubject
{
    public const int MaxSubjectLenght = 256;
    
    private static Validation Validate(string description)
    {
        if (string.IsNullOrEmpty(description))
            return Validation.Invalid(TechServiceErrors.TicketSubjectIsEmpty.ToString());
        else if (description.Length > MaxSubjectLenght)
            return Validation.Invalid(TechServiceErrors.TicketSubjectIsTooLong.ToString());

        return Validation.Ok;
    }
}