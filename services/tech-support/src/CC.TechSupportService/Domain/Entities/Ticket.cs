using CC.Shared.Domain;
using CC.TechSupportService.Domain.Enums;
using CC.TechSupportService.Domain.Events;
using CC.TechSupportService.Domain.ValueObjects.Ticket;
using FluentResults;

namespace CC.TechSupportService.Domain.Entities;

public sealed class Ticket : AggregationRoot<Guid>
{
    private Ticket(Guid id) : base(id) { }
    
    private Ticket(Guid id,
        int number,
        Reporter reporter,
        TicketSubject subject,
        TicketDescription description,
        TicketCategory ticketCategory,
        RelatedEntity? relatedEntity,
        DateTime createdAt) : base(id)
    {
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
        
        AddDomainEvent(new TicketCreated(Id));
    }
    
    /// <summary>
    /// Человекочитаемый номер заявки, пр: SUP-0001
    /// </summary>
    public int Number { get; private set; }
    public Reporter Reporter { get; private set; }
    public Guid? AssigneeId { get; private set; }
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

    public static Result<Ticket> TryCreate(Guid id,
        int number, 
        Reporter reporter, 
        TicketSubject subject, 
        TicketDescription description,
        TicketCategory ticketCategory,
        RelatedEntity? relatedEntity,
        DateTime createdAt)
    {
        //TODO: Исправить баг. Result.Fail не возвращается, ошибка игнорируется и выполнение продолжается
        if (number < 0)
            Result.Fail(TechServiceErrors.NegativeTicketNumber);
        
        return Result.Ok(new Ticket(id, number, reporter, subject, description, ticketCategory, relatedEntity, createdAt));
    }
    
    public Result ChangeAssignee(Guid assigneeId, DateTime updatedAt)
    {
        if (AssigneeId is null)
            return Result.Fail(TechServiceErrors.ChangingAgentThatHasNotBeenAssigned);
        if (AssigneeId!.Equals(assigneeId))
            return Result.Fail(TechServiceErrors.SettingTheSameAssignedTicketAgent);

        AddDomainEvent(new AssigneeChanged(AssigneeId.Value, assigneeId));
        AssigneeId = assigneeId;
        
        return Result.Ok();
    }
    
    public Result SetFirstAssigneeRespondedTime(DateTime responseTime, DateTime updatedAt)
    {
        if (FirstRespondedAt is not null)
            return Result.Fail(TechServiceErrors.SettingFirstAgentRespondedTimeForTheSecondTime);
        
        FirstRespondedAt = responseTime;
        return Result.Ok();
    }

    public Result SetLastUserRespondTime(DateTime lastTime, DateTime updatedAt)
    {
        if (LastUserRespondedAt > lastTime)
            return Result.Fail(TechServiceErrors.LastUserRespondedTimeIsBeforeCurrent);
        
        LastUserRespondedAt = lastTime;
        
        return Result.Ok();
    }
    
    public Result ChangeDescription(TicketDescription newDescription, DateTime updatedAt)
    {
        if (Description.Equals(newDescription))
            return Result.Fail(TechServiceErrors.SettingTheSameTicketDescription);
        
        var timeDifference = DateTime.UtcNow.Subtract(CreatedAt);
        if (timeDifference.Days >= 3)
            return Result.Fail(TechServiceErrors.TimeToEditTicketDescriptionHasPassed);
        
        Description = newDescription;

        return Result.Ok();
    }
    
    public Result Reclassify(TicketCategory @new, DateTime updatedAt)
    {
        if (AssigneeId is null)
            return Result.Fail(TechServiceErrors.ReclassifyingTicketWithUnassignedAgent);

        SlaFirstResponseAt = updatedAt.Add(@new.FirstResponseDeadlineInHours);
        TicketCategory = @new;
        
        AddDomainEvent(new TicketCategoryReclassified(Id, TicketCategory, @new, AssigneeId.Value, updatedAt));
        return Result.Ok();
    }
    
    public Result LeaveRatingAndComment(CsatRating csatRating, ValueObjects.Ticket.CsatComment? comment, DateTime updatedAt)
    {
        if (CsatRating is not null)
            return Result.Fail(TechServiceErrors.ChangingRatingAndCommentAfterPosting);
        
        CsatRating = csatRating;
        CsatComment = comment;
        CsatRating = csatRating;
        
        return Result.Ok();
    }
    
    public Result Open(Guid assigneeId, DateTime updatedAt)
    {
        var opened = new Opened();
        if (!Status.CanTransitionTo(opened))
            return Result.Fail(TechServiceErrors.OpeningFromStatus(Status.GetType().Name));

        AssigneeId = assigneeId;
        var oldStatus = Status;
        Status = opened;
        AddDomainEvent(new TicketEvents(Id, oldStatus));
        return Result.Ok();
    }

    public Result Progress(DateTime updatedAt)
    {
        var inProgress = new InProgress();
        if (!Status.CanTransitionTo(inProgress))
            return Result.Fail(TechServiceErrors.AssigningInProgressFromStatus(Status.GetType().Name));

        var oldStatus = Status;
        Status = inProgress;
        AddDomainEvent(new TicketProgressed(Id, oldStatus));

        return Result.Ok();
    }

    public Result ShiftToWaitingForUser(DateTime updatedAt)
    {
        var waitingForUser = new WaitingForUser();
        if (!Status.CanTransitionTo(waitingForUser))
            return Result.Fail(TechServiceErrors.ShiftingToWaitingForUserFromStatus(Status.GetType().Name));

        var oldStatus = Status;
        Status = waitingForUser;
        AddDomainEvent(new TicketShiftedToWaitingForUser(Id, oldStatus));
        
        return Result.Ok();
    }

    public Result Escalate(DateTime updatedAt)
    {
        var escalated = new Escalated();
        if (!Status.CanTransitionTo(escalated))
            return Result.Fail(TechServiceErrors.EscalatingFromStatus(Status.GetType().Name));

        var oldStatus = Status;
        Status = escalated;
        AddDomainEvent(new TicketEscalated(Id, oldStatus));
        
        return Result.Ok();
    }

    public Result Resolve(DateTime updatedAt, DateTime resolvedAt)
    {
        var resolved = new Resolved();
        if (!Status.CanTransitionTo(resolved))
            return Result.Fail(TechServiceErrors.ResolvingFromStatus(Status.GetType().Name));

        var oldStatus = Status;
        Status = resolved;
        ResolvedAt = resolvedAt;
        AddDomainEvent(new TicketResolved(Id, oldStatus));
        
        return Result.Ok();
    }

    public Result Reopen(DateTime updatedAt)
    {
        var reopened = new Reopened();
        if (!Status.CanTransitionTo(reopened))
            return Result.Fail(TechServiceErrors.ReopeningFromStatus(Status.GetType().Name));

        var oldStatus = Status;
        Status = reopened;
        AddDomainEvent(new TicketReopened(Id, oldStatus));
        
        return Result.Ok();
    }

    public Result Close(DateTime updatedAt, DateTime closedAt)
    {
        var closed = new Closed();
        if (!Status.CanTransitionTo(closed))
            return Result.Fail(TechServiceErrors.ClosingFromStatus(Status.GetType().Name));

        var oldStatus = Status;
        ClosedAt = closedAt;
        Status = closed;
        AddDomainEvent(new TicketClosed(Id, oldStatus));
        
        return Result.Ok();
    }
}