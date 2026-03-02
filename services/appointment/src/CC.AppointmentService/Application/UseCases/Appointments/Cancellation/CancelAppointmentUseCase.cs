using CC.AppointmentService.Application.Dependencies.UnitOfWork;
using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Appointments.Repositories;
using CC.AppointmentService.Domain.Appointments.Rules;
using CC.AppointmentService.Domain.Errors;
using FluentResults;
using Mediator;

namespace CC.AppointmentService.Application.UseCases.Appointments.Cancellation;

public class CancelAppointment : IRequest<Result>
{
    public Guid AppointmentId { get; init; }
    public Guid ClientId { get; init; }
    public string Reason { get; init; }
}

public class CancelAppointmentUseCase(
    IUnitOfWork unitOfWork,
    IAppointmentsRepository appointmentsRepository,
    TimeProvider timeProvider) : IRequestHandler<CancelAppointment, Result>
{
    public async ValueTask<Result> Handle(CancelAppointment command, CancellationToken cancellationToken)
    {
        var appointment = await appointmentsRepository.GetByIdAsync(command.AppointmentId, command.ClientId);
        if (appointment is null)
            return Result.Fail(AppointmentErrors.NotFound());

        if (appointment.Status != AppointmentStatus.Planned)
            return Result.Fail(AppointmentErrors.InvalidStatusForCancel());

        if (!AppointmentsTimeRules.IsCancellationAllowed(appointment.TimeSlot.From, timeProvider.GetUtcNow().DateTime))
            return Result.Fail(AppointmentErrors.CancellationDeadlineExceeded(
                AppointmentsTimeRules.MinHoursBeforeCancellation));

        appointment.Status = AppointmentStatus.Cancelled;
        appointment.CancellationReason = command.Reason;

        await unitOfWork.SaveAsync(cancellationToken);
        return Result.Ok();
    }
}