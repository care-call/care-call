using CC.Shared.Domain;
using CC.TechSupportService.Domain.Enums;
using FluentResults;

namespace CC.TechSupportService.Domain.Entities;

public sealed class TicketHistory : Entity<Guid>
{
    private TicketHistory(Guid id) : base(id) { }

    private TicketHistory(Guid id, Guid ticketId, Guid actorId, ActionType actionType, string? oldValue, string? newValue, DateTime createdAt) : base(id)
    {
        TicketId = ticketId;
        ActorId = actorId;
        ActionType = actionType;
        OldValue = oldValue;
        NewValue = newValue;
        CreatedAt = createdAt;
    }
    
    public Guid TicketId { get; private set; }

    /// <summary>
    /// Кто сделал изменения
    /// </summary>
    public Guid ActorId { get; private set; }

    public ActionType ActionType  { get; private set; }

    public string? OldValue { get; private set; }

    public string? NewValue { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public static Result<TicketHistory> TryCreate(Guid id, 
        Guid ticketId, 
        Guid actorId, 
        ActionType action, 
        string? oldValue, 
        string? newValue, 
        DateTime createdAt)
    {
        if (ticketId.Equals(actorId))
            return Result.Fail(TechServiceErrors.PassingTheSameTicketIdAsActorId);
        
        return Result.Ok(new TicketHistory(id, ticketId, actorId, action, oldValue, newValue, createdAt));
    }
}