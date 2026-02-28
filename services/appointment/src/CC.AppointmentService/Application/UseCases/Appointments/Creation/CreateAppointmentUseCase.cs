using CC.AppointmentService.Application.Dependencies.UnitOfWork;
using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Appointments.Repositories;
using CC.AppointmentService.Domain.Appointments.Rules;
using CC.AppointmentService.Domain.Errors;
using CC.Shared.Domain.TimeRanges;
using FluentResults;
using Mediator;

namespace CC.AppointmentService.Application.UseCases.Appointments.Creation;

public record CreateAppointment : IRequest<Result>
{
    public required Guid ClientId { get; init; }
    public required Guid PractitionerId { get; init; }
    public required DateTimeRange TimeSlot { get; init; }
    public required ClientSnapshot ClientSnapshot { get; init; }
    public required PractitionerSnapshot PractitionerSnapshot { get; init; }
}

public class CreateAppointmentUseCase(
    IUnitOfWork unitOfWork,
    IAppointmentsRepository appointmentsRepository,
    TimeProvider timeProvider) : IRequestHandler<CreateAppointment, Result>
{
    public async ValueTask<Result> Handle(CreateAppointment command, CancellationToken cancellationToken)
    {
        if (AppointmentsTimeRules.IsTooSoon(command.TimeSlot.From, timeProvider.GetUtcNow().DateTime))
            return Result.Fail(AppointmentErrors.TooSoon(AppointmentsTimeRules.MinHoursBeforeStart));

        var hasIntercept = await appointmentsRepository.HasInterceptsAsync(command.TimeSlot, command.ClientId);
        if (hasIntercept)
            return Result.Fail(AppointmentErrors.HasIntercepts());

        var lastAppointment = await appointmentsRepository.GetLastAppointmentAsync(command.ClientId);
        if (lastAppointment != null &&
            AppointmentsTimeRules.HasInsufficientBreak(lastAppointment.TimeSlot.To, command.TimeSlot.From))
            return Result.Fail(AppointmentErrors.MinBreakBetweenAppointments(
                AppointmentsTimeRules.MinBreakBetweenAppointmentsMinutes));

        var appointment = new Appointment(Guid.CreateVersion7())
        {
            ClientId = command.ClientId,
            PractitionerId = command.PractitionerId,
            TimeSlot = command.TimeSlot,
            Status = AppointmentStatus.Planned,
            ClientSnapshot = command.ClientSnapshot,
            PractitionerSnapshot = command.PractitionerSnapshot
        };

        await appointmentsRepository.AddAsync(appointment);
        await unitOfWork.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}