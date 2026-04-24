using CC.Shared.Domain;
using CC.TechSupportService.Domain.Events;
using CC.TechSupportService.Domain.ValueObjects.Ticket;
using FluentResults;

namespace CC.TechSupportService.Domain.Entities;

public class Ticket : AggregationRoot<GuidId>
{
    private List<TicketAttachment> _attachments;

    private List<TicketComment> _comments;
    
    private Ticket(GuidId id) : base(id) { }
    
    private Ticket(GuidId id,
        List<TicketAttachment> attachments,
        List<TicketComment> comments,
        int number,
        Reporter reporter,
        TicketSubject subject,
        TicketDescription description,
        TicketCategory ticketCategory,
        RelatedEntity? relatedEntity,
        DateTime createdAt) : base(id)
    {
        _attachments = attachments;
        _comments = comments;
        Number = number;
        Reporter = reporter;
        Subject = subject;
        Description = description;
        TicketCategory = ticketCategory;
        Status = new New();
        RelatedEntity = relatedEntity;
        CreatedAt = createdAt;
        UpdatedAt = CreatedAt;

        SlaFirstResponseAt = createdAt.Add(TicketCategory.FirstResponseDeadlineInHours);
        
        AddDomainEvent(new TicketCreated(Id.Value));
    }
    
    /// <summary>
    /// Человекочитаемый номер заявки, пр: SUP-0001
    /// </summary>
    public int Number { get; private set; }

    public IReadOnlyList<TicketAttachment> Attachments => _attachments;

    public IReadOnlyList<TicketComment> Comments => _comments;
    
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

    public DateTime? ResolvedAt { get; private set; }

    public DateTime? ClosedAt { get; private set; }

    public CsatRating? CsatRating { get; private set; }

    public CsatComment? CsatComment { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public static Result<Ticket> TryCreate(GuidId id,
        List<TicketAttachment> attachments,
        List<TicketComment> ticketComments,
        int number, 
        Reporter reporter, 
        TicketSubject subject, 
        TicketDescription description,
        TicketCategory ticketCategory,
        RelatedEntity? relatedEntity,
        DateTime createdAt)
    {
        var errors = new List<string>();
        
        if(number < 0)
             errors.Add("number cannot be less than 0!");
        if(createdAt > DateTime.UtcNow)
            errors.Add("created at time cannot be in the future");

        if (errors.Count is not 0)
            return Result.Fail(errors);
        
        return Result.Ok(new Ticket(id, attachments, ticketComments, number, reporter, subject, description, ticketCategory, relatedEntity, createdAt));
    }

    public Result AddAttachments(List<TicketAttachment> itemsToAdd, DateTime updatedAt)
    {
        if (_attachments.Any(itemsToAdd.Contains))
            return Result.Fail("cannot add elements that are currently added");
            
        _attachments.AddRange(itemsToAdd);
        UpdatedAt = updatedAt;
        
        return Result.Ok();
    }
    
    public Result RemoveAttachments(List<TicketAttachment> itemsToRemove, DateTime updatedAt)
    {
        if (!itemsToRemove.All(_attachments.Contains))
            return Result.Fail("At least one element is absent in the items to remove");

        foreach (var item in itemsToRemove)
        {
            _attachments.Remove(item);
        }
        UpdatedAt = updatedAt;
        
        return Result.Ok();
    }
    
    public Result AddComments(List<TicketComment> itemsToAdd, DateTime updatedAt)
    {
        if (_comments.Any(itemsToAdd.Contains))
            return Result.Fail("Cannot add elements that are currently added");
            
        _comments.AddRange(itemsToAdd);
        UpdatedAt = updatedAt;
        
        return Result.Ok();
    }
    
    public Result RemoveComments(List<TicketComment> itemsToRemove, DateTime updatedAt)
    {
        if (!itemsToRemove.All(_comments.Contains))
            return Result.Fail("At least one element is absent in the items to remove");

        foreach (var item in itemsToRemove)
        {
            _comments.Remove(item);
        }
        UpdatedAt = updatedAt;
        
        return Result.Ok();
    }
    
    public Result ChangeAssignee(GuidId assignee, DateTime updatedAt)
    {
        if (AssigneeId is null)
            return Result.Fail("Cannot change assignee when it hasn't been assigned");
        if (AssigneeId!.Equals(assignee))
            return Result.Fail("New assignee is the same as the current");

        AddDomainEvent(new AssigneeChanged(AssigneeId.Value, assignee.Value));
        AssigneeId = assignee;
        UpdatedAt = updatedAt;
        
        return Result.Ok();
    }
    
    public Result SetFirstAssigneeRespondedTime(DateTime responseTime, DateTime updatedAt)
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
            return Result.Fail("New responded time cannot be before than the current is!");

        UpdatedAt = updatedAt;
        LastUserRespondedAt = lastTime;
        
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

        SlaFirstResponseAt = updatedAt.Add(@new.FirstResponseDeadlineInHours);
        
        TicketCategory = @new;
        UpdatedAt = updatedAt;
        
        AddDomainEvent(new TicketCategoryReclassified(Id.Value, TicketCategory, @new, AssigneeId.Value, updatedAt));
        return Result.Ok();
    }
    
    public Result LeaveRatingAndComment(CsatRating csatRating, ValueObjects.Ticket.CsatComment? comment, DateTime updatedAt)
    {
        if (CsatRating is not null)
            return Result.Fail("Cannot change Rating and comment after posting");
        
        CsatRating = csatRating;
        CsatComment = comment;
        UpdatedAt = updatedAt;
        CsatRating = csatRating;
        
        return Result.Ok();
    }
    
    public Result Open(GuidId assigneeId, DateTime updatedAt)
    {
        var opened = new Opened();
        if (!Status.CanTransitionTo(opened))
            return Result.Fail($"Cannot open from {Status.GetType().Name}");

        AssigneeId = assigneeId;
        var oldStatus = Status;
        UpdatedAt = updatedAt;
        Status = opened;
        AddDomainEvent(new TicketEvents(Id.Value, oldStatus));
        return Result.Ok();
    }

    public Result Progress(DateTime updatedAt)
    {
        var inProgress = new InProgress();
        if (!Status.CanTransitionTo(inProgress))
            return Result.Fail($"Cannot assign in progress from {Status.GetType().Name}");

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
            return Result.Fail($"Cannot Shift to waiting for user from {Status.GetType().Name}");

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
            return Result.Fail($"Cannot escalate from {Status.GetType().Name}");

        var oldStatus = Status;
        UpdatedAt = updatedAt;
        Status = escalated;
        AddDomainEvent(new TicketEscalated(Id.Value, oldStatus));
        
        return Result.Ok();
    }

    public Result Resolve(DateTime updatedAt, DateTime resolvedAt)
    {
        var resolved = new Resolved();
        if (!Status.CanTransitionTo(resolved))
            return Result.Fail($"Cannot resolve from {Status.GetType().Name}");

        var oldStatus = Status;
        UpdatedAt = updatedAt;
        Status = resolved;
        ResolvedAt = resolvedAt;
        AddDomainEvent(new TicketResolved(Id.Value, oldStatus));
        
        return Result.Ok();
    }

    public Result Reopen(DateTime updatedAt)
    {
        var reopened = new Reopened();
        if (!Status.CanTransitionTo(reopened))
            return Result.Fail($"Cannot reopen from {Status.GetType().Name}");

        var oldStatus = Status;
        UpdatedAt = updatedAt;
        Status = reopened;
        AddDomainEvent(new TicketReopened(Id.Value, oldStatus));
        
        return Result.Ok();
    }

    public Result Close(DateTime updatedAt, DateTime closedAt)
    {
        var closed = new Closed();
        if (!Status.CanTransitionTo(closed))
            return Result.Fail($"Cannot close from {Status.GetType().Name}");

        var oldStatus = Status;
        UpdatedAt = updatedAt;
        ClosedAt = closedAt;
        Status = closed;
        AddDomainEvent(new TicketClosed(Id.Value, oldStatus));
        
        return Result.Ok();
    }
}