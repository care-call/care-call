using CC.AppointmentService.Application.Dependencies.UnitOfWork;
using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Appointments.Repositories;
using CC.AppointmentService.Domain.Appointments.Rules;
using CC.AppointmentService.Domain.Errors;
using CC.Shared.Domain.TimeRanges;
using FluentResults;
using Mediator;

namespace CC.AppointmentService.Application.UseCases.Appointments.Transferring;

public record TransferAppointment : IRequest<Result>
{
    public Guid AppointmentId { get; init; }
    public DateTimeRange TimeSlot { get; init; }
    public Guid ClientId { get; init; }
}

public class TransferAppointmentUseCase(
    IAppointmentsRepository appointmentsRepository,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider)
    : IRequestHandler<TransferAppointment, Result>
{
    public async ValueTask<Result> Handle(TransferAppointment command, CancellationToken cancellationToken)
    {
        var appointment = await appointmentsRepository.GetByIdAsync(command.AppointmentId, command.ClientId);
        if (appointment is null)
            return Result.Fail(AppointmentErrors.NotFound());

        if (appointment.Status != AppointmentStatus.Planned)
            return Result.Fail(AppointmentErrors.InvalidStatusForTransfer());

        var currentTime = timeProvider.GetUtcNow().DateTime;

        if (!AppointmentsTimeRules.IsWithinAllowedShift(appointment.TimeSlot.From, command.TimeSlot.From))
            return Result.Fail(AppointmentErrors.NotWithinAllowedShift(AppointmentsTimeRules.MaxTransferringShiftDays));

        if (AppointmentsTimeRules.IsTransferAllowed(command.TimeSlot.From, currentTime))
            return Result.Fail(AppointmentErrors.TooSoon(AppointmentsTimeRules.MinHoursBeforeStart));

        var hasIntersections = await appointmentsRepository.HasInterceptsAsync(command.TimeSlot, appointment.ClientId);
        if (hasIntersections)
            return Result.Fail(AppointmentErrors.HasIntercepts());

        appointment.TimeSlot = command.TimeSlot;
        await unitOfWork.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}