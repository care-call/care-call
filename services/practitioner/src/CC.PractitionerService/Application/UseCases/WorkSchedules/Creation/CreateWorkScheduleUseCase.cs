using CC.PractitionerService.Application.Dependencies.UnitOfWork;
using CC.PractitionerService.Domain.WorkSchedules;
using CC.PractitionerService.Domain.WorkSchedules.Repositories;
using FluentResults;
using Mediator;

namespace CC.PractitionerService.Application.UseCases.WorkSchedules.Creation;

public record CreateWorkSchedule : IRequest<Result>
{
    public required Guid PractitionerId { get; init; }
    public required WorkScheduleValidityPeriod ValidityPeriod { get; init; }
    public required SessionDuration SessionDuration { get; init; }
    public required string TimeZoneId { get; init; }
    public required IReadOnlyList<WeeklyRecurrence> Recurrences { get; init; } = [];
}

public class CreateWorkScheduleUseCase(
    TimeProvider timeProvider,
    IUnitOfWork unitOfWork,
    IWorkScheduleRepository workScheduleRepository) : IRequestHandler<CreateWorkSchedule, Result>
{
    public async ValueTask<Result> Handle(CreateWorkSchedule command, CancellationToken cancellationToken)
    {
        if (command.ValidityPeriod.From < DateOnly.FromDateTime(timeProvider.GetUtcNow().Date))
            return Result.Fail("Прошедшая дата недопустима для начала графика работы");
        
        var currentActive = await workScheduleRepository.GetActiveForPractitionerAsync(command.PractitionerId);
        if (currentActive is not null)
        {
            if (command.ValidityPeriod.From <= currentActive.ValidityPeriod.From)
                return Result.Fail("Новое расписание должно начинаться после текущего");
            currentActive.EndOn(command.ValidityPeriod.From);
        }

        var newSchedule = new WorkSchedule(
            Guid.CreateVersion7(),
            practitionerId: command.PractitionerId,
            validityPeriod: command.ValidityPeriod,
            timeZone: TimeZoneInfo.FindSystemTimeZoneById(command.TimeZoneId),
            recurrences: command.Recurrences.ToList())
        {
            SessionDuration = command.SessionDuration
        };

        await workScheduleRepository.AddAsync(newSchedule);

        await unitOfWork.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}