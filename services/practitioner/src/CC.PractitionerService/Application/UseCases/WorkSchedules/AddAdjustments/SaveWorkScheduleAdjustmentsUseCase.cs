using CC.PractitionerService.Application.Dependencies.UnitOfWork;
using CC.PractitionerService.Application.UseCases.WorkSchedules.Dtos;
using CC.PractitionerService.Domain.WorkSchedules;
using CC.PractitionerService.Domain.WorkSchedules.Extensions;
using CC.PractitionerService.Domain.WorkSchedules.Repositories;
using CC.Shared.Domain.TimeRanges;
using FluentResults;
using Mediator;

namespace CC.PractitionerService.Application.UseCases.WorkSchedules.AddAdjustments;

public record SaveWorkScheduleAdjustments : IRequest<Result>
{
    public required IReadOnlyCollection<CreateAdjustmentDto> NewAdjustments { get; init; }
    public required IReadOnlyCollection<Guid> RemovedAdjustments { get; init; }
    public required DateTime WeeklyStartDate { get; init; }
    public required Guid WorkScheduleId { get; init; }
}

public sealed class SaveWorkScheduleAdjustmentsUseCase(
    IAdjustmentRepository adjustmentRepository,
    IUnitOfWork unitOfWork,
    IWorkScheduleRepository workScheduleRepository
) : IRequestHandler<SaveWorkScheduleAdjustments, Result>
{
    public async ValueTask<Result> Handle(SaveWorkScheduleAdjustments command, CancellationToken cancellationToken)
    {
        var schedule = await workScheduleRepository.GetAsync(command.WorkScheduleId);
        if (schedule is null)
            return Result.Fail("График не найден.");

        var weeklyAdjustments =
            await adjustmentRepository.GetWeeklyAsync(schedule.Id, new Week(command.WeeklyStartDate));

        if (command.RemovedAdjustments.Count != 0)
        {
            var removedAdjustments =
                weeklyAdjustments.Where(a => command.RemovedAdjustments.Contains(a.Id)).ToList();
            adjustmentRepository.Remove(removedAdjustments);
            weeklyAdjustments = weeklyAdjustments.Where(a => !removedAdjustments.Contains(a)).ToList();
        }

        if (command.NewAdjustments.Count != 0)
        {
            var newAdjustments = command.NewAdjustments.Select(a => new Adjustment(Guid.CreateVersion7(),
                schedule.Id, a.AdjustmentType, new DateTimeRange(a.StartDate, a.EndDate))).ToList();
            
            var week = new Week(command.WeeklyStartDate);
            if (newAdjustments.Any(a => !week.IsInclusionPeriod(a.Period)))
                return Result.Fail("Корректировка вне недели");
            
            if (newAdjustments.Any(adj => !schedule.IsActiveOn(adj.Period)))
                return Result.Fail("Корректировка вне действия графика");
            
            weeklyAdjustments.AddRange(newAdjustments);
            weeklyAdjustments = weeklyAdjustments.ClearDuplicates().ToList();
            
            var conflicts = weeklyAdjustments.DetermineConflicts();
            if (conflicts.Any())
                return Result.Fail("Конфликт с существующей корректировкой");
            
            var newAdjustmentsToAdd = weeklyAdjustments.Where(a => newAdjustments.Contains(a)).ToList();
            await adjustmentRepository.AddAsync(newAdjustmentsToAdd);
        }
        
        await unitOfWork.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}