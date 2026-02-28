using CC.PractitionerService.Api.Contracts.Common;
using CC.PractitionerService.Application.Dependencies.UnitOfWork;
using CC.PractitionerService.Domain.WorkSchedules;
using CC.PractitionerService.Domain.WorkSchedules.Repositories;
using CC.Shared.Domain.TimeRanges;
using FluentResults;
using Mediator;

namespace CC.PractitionerService.Application.UseCases.WorkSchedules.AddAdjustments;

public record CreateAdjustmentsForSchedule : IRequest<Result>
{
    public required Guid WorkScheduleId { get; init; }
    public required ICollection<AdjustmentDto> Adjustments { get; init; }
}

public sealed class AddAdjustmentsUseCase(
    IAdjustmentRepository adjustmentRepository,
    IUnitOfWork unitOfWork,
    IWorkScheduleRepository workScheduleRepository
) : IRequestHandler<CreateAdjustmentsForSchedule, Result>
{
    public async ValueTask<Result> Handle(CreateAdjustmentsForSchedule command, CancellationToken cancellationToken)
    {
        if (command.Adjustments.Count == 0)
            return Result.Fail("Список корректировок пуст.");

        var schedule = await workScheduleRepository.GetAsync(command.WorkScheduleId);
        if (schedule is null)
            return Result.Fail("График не найден.");

        var adjustments = command.Adjustments.Select(a => new Adjustment
        {
            WorkScheduleId =  command.WorkScheduleId,
            Description = null,
            Type = a.AdjustmentType,
            Period = new DateTimeRange(a.StartDate, a.EndDate)
        }).ToList();
        
        if (adjustments.Any(adj => !schedule.IsActiveOn(DateOnly.FromDateTime(adj.Period.From))))
        {
            return Result.Fail("Период корректировки не соответствует периоду действия графика.");
        }
     
        var ordered = adjustments
            .OrderBy(a => a.Period.From)
            .ThenBy(a => a.Period.To)
            .ToList();
   
        for (int i = 0; i < ordered.Count - 1; i++)
        {
            var current = ordered[i];
            var next = ordered[i + 1];
            
            if (Intersects(current.Period, next.Period))
            {
                return Result.Fail(
                    $"Корректировки пересекаются: ");
            }
          
            if (current.Type == next.Type &&
                current.Period.From == next.Period.From &&
                current.Period.To == next.Period.To)
            {
                return Result.Fail("Дубликат корректировки.");
            }
        }
        
        var existing = await adjustmentRepository.ListByScheduleIdAsync(command.WorkScheduleId);
        
        if (existing.Count > 0)
        {
            var merged = existing
                .Concat(ordered)
                .OrderBy(a => a.Period.From)
                .ThenBy(a => a.Period.To)
                .ToList();

            for (int i = 0; i < merged.Count - 1; i++)
            {
                if (Intersects(merged[i].Period, merged[i + 1].Period))
                    return Result.Fail("Новая корректировка конфликтует с уже существующей.");
            }
        }
        await adjustmentRepository.AddAsync(ordered);
        await unitOfWork.SaveAsync(cancellationToken);

        return Result.Ok();
    }

    private bool Intersects(DateTimeRange a, DateTimeRange b)
        => a.From < b.To && b.From < a.To;
}