using CC.AppointmentService.Application.Dependencies.UnitOfWork;
using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Appointments.Repositories;
using CC.AppointmentService.Domain.Appointments.Rules;
using CC.AppointmentService.Domain.Errors;
using CC.Shared.Domain.TimeRanges;
using FluentResults;
using Mediator;

namespace CC.AppointmentService.Application.UseCases.Appointments.Transferring;

public sealed record TransferAppointment : IRequest<Result>
{
    public required Guid AppointmentId { get; init; }
    public required DateTimeRange TimeSlot { get; init; }
    public required Guid ClientId { get; init; }
}

public class TransferAppointmentUseCase(
    IAppointmentsRepository appointmentsRepository,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider)
    : IRequestHandler<TransferAppointment, Result>
{
    public async ValueTask<Result> Handle(TransferAppointment command, CancellationToken cancellationToken)
    {
        var appointment = await appointmentsRepository.GetByIdAsync(command.AppointmentId);
        if (appointment is null || appointment.ClientId != command.ClientId)
            return Result.Fail(AppointmentErrors.NotFound());

        if (appointment.Status != AppointmentStatus.Planned)
            return Result.Fail(AppointmentErrors.InvalidStatusForTransfer());

        var currentTime = timeProvider.GetUtcNow().DateTime;

        if (!AppointmentsTimeRules.IsWithinAllowedShift(appointment, command.TimeSlot.From))
            return Result.Fail(AppointmentErrors.NotWithinAllowedShift);

        appointment.TimeSlot = command.TimeSlot;
        
        if (AppointmentsTimeRules.IsTransferAllowed(appointment, currentTime))
            return Result.Fail(AppointmentErrors.TooSoon);

        var hasIntersections = await appointmentsRepository.HasInterceptsAsync(appointment);
        if (hasIntersections)
            return Result.Fail(AppointmentErrors.HasIntercepts());

        await unitOfWork.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}