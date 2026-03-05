using CC.PractitionerService.Api.Contracts.Common;
using CC.PractitionerService.Application.Dependencies.UnitOfWork;
using CC.PractitionerService.Domain.WorkSchedules;
using CC.PractitionerService.Domain.WorkSchedules.Repositories;
using CC.Shared.Domain.TimeRanges;
using FluentResults;
using Mediator;

namespace CC.PractitionerService.Application.UseCases.WorkSchedules.AddAdjustments;

public record SaveAdjustmentsForSchedule : IRequest<Result>
{
    public required ICollection<AdjustmentDto> NewAdjustments { get; init; }
    public required ICollection<Guid> RemovedAdjustments { get; init; }
    public required DateTime WeeklyStartDate { get; init; }
    public required Guid WorkScheduleId { get; init; }
}

public sealed class SaveAdjustmentsUseCase(
    IAdjustmentRepository adjustmentRepository,
    IUnitOfWork unitOfWork,
    IWorkScheduleRepository workScheduleRepository
) : IRequestHandler<SaveAdjustmentsForSchedule, Result>
{
    public async ValueTask<Result> Handle(SaveAdjustmentsForSchedule command, CancellationToken cancellationToken)
    {
        var schedule = await workScheduleRepository.GetAsync(command.WorkScheduleId);
        if (schedule is null)
            return Result.Fail("График не найден.");

        var weeklyAdjustments =
            await adjustmentRepository.GetWeeklyAsync(schedule.Id, new Week(command.WeeklyStartDate));
        
        if (command.RemovedAdjustments.Count != 0)
            adjustmentRepository.Remove(
                weeklyAdjustments.Where(a => command.RemovedAdjustments.Contains(a.Id)).ToList());
        
        if (command.NewAdjustments.Count != 0)
        {
            var newAdjustments = command.NewAdjustments.Select(a => new Adjustment(Guid.CreateVersion7(),
                schedule.Id, a.AdjustmentType, new DateTimeRange(a.StartDate, a.EndDate))).ToList();
            
            var week = new Week(command.WeeklyStartDate);
            if (!newAdjustments.Any(a => a.Period.From >= week.StartedAt && a.Period.To <= week.EndedAt))
                return Result.Fail("Корректировка вне недели");
            
            weeklyAdjustments.AddRange(newAdjustments);
            weeklyAdjustments.DistinctBy(a => new { a.Period.From, a.Period.To, a.Type });
            
            var conflicts = weeklyAdjustments.DetermineConflicts();
            if (conflicts.Any())
                return Result.Fail("Конфликт с существующей корректировкой");
            
            if (weeklyAdjustments.Any(adj =>
                    !schedule.IsActiveOn(DateOnly.FromDateTime(adj.Period.From)) ||
                    !schedule.IsActiveOn(DateOnly.FromDateTime(adj.Period.To))))
                return Result.Fail("Корректировка вне действия графика");
            
            await adjustmentRepository.AddAsync(newAdjustments);
        }
        
        await unitOfWork.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}