using CC.Shared.Domain;
using CC.TechSupportService.Domain.Events;
using CC.TechSupportService.Domain.ValueObjects.Ticket;
using FluentResults;

namespace CC.TechSupportService.Domain.Entities;

public class Ticket : AggregationRoot<GuidId>
{
    private Ticket(GuidId id) : base(id) { }

    private Ticket(GuidId id,
        int number,
        Reporter reporter,
        GuidId? assigneeId,
        TicketSubject subject,
        TicketDescription description,
        TicketCategory ticketCategory,
        TicketStatus status,
        RelatedEntity? relatedEntity,
        DateTime? firstRespondedAt,
        DateTime? lastUserRespondedAt,
        Rating? rating,
        TicketComment? comment,
        DateTime createdAt,
        DateTime updatedAt) : base(id)
    {
        Number = number;
        Reporter = reporter;
        AssigneeId = assigneeId;
        Subject = subject;
        Description = description;
        TicketCategory = ticketCategory;
        Status = status;
        RelatedEntity = relatedEntity;
        FirstRespondedAt = firstRespondedAt;
        LastUserRespondedAt = lastUserRespondedAt;
        Rating = rating;
        Comment = comment;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        
        AddDomainEvent(new TicketCreated(Id.Value));
    }
    
    /// <summary>
    /// Человекочитаемый номер заявки, пр: SUP-0001
    /// </summary>
    public int Number { get; private set; }
    
    public Reporter Reporter { get; private set; }
    
    public GuidId? AssigneeId { get; private set; }
    
    public TicketSubject Subject { get; private set; }
    
    public TicketDescription Description { get; private set; }
    
    public TicketCategory TicketCategory { get; private set; }

    public TicketStatus Status { get; private set; }
    
    public RelatedEntity? RelatedEntity { get; set; }
    
    /// <summary>
    /// Факт первого ответа от специалиста
    /// </summary>
    public DateTime? FirstRespondedAt { get; private set; }

    /// <summary>
    /// Дедлайн первого ответа от специалиста
    /// </summary>
    public DateTime SlaFirstResponseAt { get; private set; }
    
    /// <summary>
    /// Последнее время ответа пользователя
    /// </summary>
    public DateTime? LastUserRespondedAt { get; private set; }
    
    public Rating? Rating { get; private set; }
    
    public ValueObjects.Ticket.TicketComment? Comment { get; private set; }

    public DateTime CreatedAt { get; private set; }
    
    public DateTime UpdatedAt { get; private set; }
    
    public static Result<Ticket> TryCreate(GuidId id,
        int number, 
        Reporter reporter, 
        GuidId? assigneeId, 
        TicketSubject subject, 
        TicketDescription description,
        TicketCategory ticketCategory,
        TicketStatus ticketStatus,
        RelatedEntity? relatedEntity,
        DateTime? firstRespondedAt,
        DateTime? lastUserRespondedAt,
        Rating? rating,
        TicketComment? comment,
        DateTime createdAt)
    {
        if(number < 0)
            return Result.Fail("number cannot be less than 0!");
        
        return Result.Ok(new Ticket(id, number, reporter, assigneeId, subject, description, ticketCategory, ticketStatus, relatedEntity, firstRespondedAt, lastUserRespondedAt, rating, comment, createdAt, createdAt));
    }
    
    public Result ChangeAssignee(GuidId assignee, DateTime updatedAt)
    {
        if (AssigneeId is null)
            return Result.Fail("Cannot change assignee when it hasn't been assigned");
        else if (AssigneeId!.Equals(assignee))
            return Result.Fail("New assignee is the same as current");

        AddDomainEvent(new AssigneeChanged(AssigneeId.Value, assignee.Value));
        AssigneeId = assignee;
        UpdatedAt = updatedAt;
        
        return Result.Ok();
    }
    
    public Result SetFirstRespondedTime(DateTime responseTime, DateTime updatedAt)
    {
        if (FirstRespondedAt is not null)
            return Result.Fail("First response time is already set!");

        UpdatedAt = updatedAt;
        FirstRespondedAt = responseTime;
        return Result.Ok();
    }

    public Result SetLastUserRespondTime(DateTime lastTime, DateTime updatedAt)
    {
        if (LastUserRespondedAt > lastTime)
            return Result.Fail("New responded time cannot be lower than current is!");

        UpdatedAt = updatedAt;
        
        return Result.Ok();
    }
    
    public Result ChangeDescription(TicketDescription newDescription, DateTime updatedAt)
    {
        if (Description.Equals(newDescription))
            return Result.Fail("New description is the same as previous!");
        
        var timeDifference = DateTime.UtcNow.Subtract(CreatedAt);
        if (timeDifference.Days >= 3)
        {
            return Result.Fail("Time to edit has passed!");
        }

        Description = newDescription;
        UpdatedAt = updatedAt;

        return Result.Ok();
    }
    
    public Result Reclassify(TicketCategory @new, DateTime updatedAt)
    {
        if (AssigneeId is null)
            return Result.Fail("Cannot Reclassify ticket with unassigned actor!");

        SlaFirstResponseAt = updatedAt.AddHours(@new.FirstResponseDeadlineInHours);
        
        TicketCategory = @new;
        UpdatedAt = updatedAt;
        
        AddDomainEvent(new TicketCategoryReclassified(Id.Value, TicketCategory, @new, AssigneeId.Value, updatedAt));
        return Result.Ok();
    }
    
    public Result LeaveRatingAndComment(Rating rating, ValueObjects.Ticket.TicketComment? comment, DateTime updatedAt)
    {
        if (Rating is not null)
            return Result.Fail("Cannot change Rating and comment after posting");
        
        Rating = rating;
        Comment = comment;
        UpdatedAt = updatedAt;
        Rating = rating;
        
        return Result.Ok();
    }
    
    public Result Open(GuidId assigneeId, DateTime updatedAt)
    {
        var opened = new Opened();
        if (!Status.CanTransitionTo(opened))
            return Result.Fail($"Cannot open from {Status.Value}");

        AssigneeId = assigneeId;
        var oldStatus = Status;
        UpdatedAt = updatedAt;
        Status = opened;
        AddDomainEvent(new TicketOpened(Id.Value, oldStatus));
        return Result.Ok();
    }

    public Result Progress(DateTime updatedAt)
    {
        var inProgress = new InProgress();
        if (!Status.CanTransitionTo(inProgress))
            return Result.Fail($"Cannot assign in progress from {Status}");

        var oldStatus = Status;
        UpdatedAt = updatedAt;
        Status = inProgress;
        AddDomainEvent(new TicketProgressed(Id.Value, oldStatus));

        return Result.Ok();
    }

    public Result ShiftToWaitingForUser(DateTime updatedAt)
    {
        var waitingForUser = new WaitingForUser();
        if (!Status.CanTransitionTo(waitingForUser))
            return Result.Fail($"Cannot Shift to waiting for user from {Status}");

        var oldStatus = Status;
        UpdatedAt = updatedAt;
        Status = waitingForUser;
        AddDomainEvent(new TicketShiftedToWaitingForUser(Id.Value, oldStatus));
        
        return Result.Ok();
    }

    public Result Escalate(DateTime updatedAt)
    {
        var escalated = new Escalated();
        if (!Status.CanTransitionTo(escalated))
            return Result.Fail($"Cannot Shift to escalated from {Status}");

        var oldStatus = Status;
        UpdatedAt = updatedAt;
        Status = escalated;
        AddDomainEvent(new TicketEscalated(Id.Value, oldStatus));
        
        return Result.Ok();
    }

    public Result Resolve(DateTime updatedAt, DateTime resolvedAt)
    {
        var resolved = new Resolved(resolvedAt);
        if (!Status.CanTransitionTo(resolved))
            return Result.Fail($"Cannot Shift to resolved from {Status}");

        var oldStatus = Status;
        UpdatedAt = updatedAt;
        Status = resolved;
        AddDomainEvent(new TicketResolved(Id.Value, oldStatus));
        
        return Result.Ok();
    }

    public Result Reopen(DateTime updatedAt)
    {
        var reopened = new Reopened();
        if (!Status.CanTransitionTo(reopened))
            return Result.Fail($"Cannot Shift to reopened from {Status}");

        var oldStatus = Status;
        UpdatedAt = updatedAt;
        Status = reopened;
        AddDomainEvent(new TicketReopened(Id.Value, oldStatus));
        
        return Result.Ok();
    }

    public Result Close(DateTime updatedAt, DateTime closedAt)
    {
        var closed = new Closed(closedAt);
        if (!Status.CanTransitionTo(closed))
            return Result.Fail($"Cannot Shift to closed from {Status}");

        var oldStatus = Status;
        UpdatedAt = updatedAt;
        Status = closed;
        AddDomainEvent(new TicketClosed(Id.Value, oldStatus));
        
        return Result.Ok();
    }
}