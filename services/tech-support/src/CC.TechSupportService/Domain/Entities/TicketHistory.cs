using CC.Shared.Domain;
using CC.TechSupportService.Domain.Enums;
using FluentResults;

namespace CC.TechSupportService.Domain.Entities;

public class TicketHistory : Entity<GuidId>
{
    private TicketHistory(GuidId id) : base(id) { }

    private TicketHistory(GuidId id, GuidId ticketId, GuidId actorId, ActionType action, string? oldValue, string? newValue, DateTime createdAt) : base(id)
    {
        TicketId = ticketId;
        ActorId = actorId;
        Action = action;
        OldValue = oldValue;
        NewValue = newValue;
        CreatedAt = createdAt;
    }
    
    public GuidId TicketId { get; private set; }

    /// <summary>
    /// Кто сделал изменения
    /// </summary>
    public GuidId ActorId { get; private set; }

    public ActionType Action  { get; private set; }

    public string? OldValue { get; private set; }

    public string? NewValue { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public static Result<TicketHistory> TryCreate(GuidId id, 
        GuidId ticketId, 
        GuidId actorId, 
        ActionType action, 
        string? oldValue, 
        string? newValue, 
        DateTime createdAt)
    {
        if(ticketId.Equals(actorId))
            return Result.Fail("Ticket id cannot be the same as actor id!");
        
        return Result.Ok(new TicketHistory(id, ticketId, actorId, action, oldValue, newValue, createdAt));
    }
}