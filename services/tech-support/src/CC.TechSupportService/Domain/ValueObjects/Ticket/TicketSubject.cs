using FluentResults;

namespace CC.TechSupportService.Domain.ValueObjects.Ticket;

public class TicketSubject
{
    public const int MaxSubjectLenght = 256;
    
    private TicketSubject(string subject) => Subject = subject;

    public static Result<TicketSubject> TryCreate(string description)
    {
        if (string.IsNullOrEmpty(description))
            return Result.Fail("Subject cannot be null or empty");
        else if (description.Length > MaxSubjectLenght)
            return Result.Fail($"Subject cannot be greater than {MaxSubjectLenght}");
        
        return Result.Ok(new TicketSubject(description));
    }
    
    public string Subject { get; private set; }
}