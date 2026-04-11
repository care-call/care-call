using CC.AppointmentService.Application.Dependencies.UnitOfWork;
using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Appointments.Repositories;
using CC.AppointmentService.Domain.Appointments.Rules;
using CC.AppointmentService.Domain.Errors;
using FluentResults;

namespace CC.AppointmentService.Application.UseCases.Appointments.Cancellation;

public sealed record CancelAppointment
{
    public required Guid AppointmentId { get; init; }
    public required Guid ClientId { get; init; }
    public required string Reason { get; init; }
}

public class CancelAppointmentUseCase(
    IUnitOfWork unitOfWork,
    IAppointmentsRepository appointmentsRepository,
    TimeProvider timeProvider)
{
    public async ValueTask<Result> Handle(CancelAppointment command, CancellationToken cancellationToken)
    {
        var appointment = await appointmentsRepository.GetByIdAsync(command.AppointmentId);
        if (appointment is null || appointment.ClientId != command.ClientId)
            return Result.Fail(AppointmentErrors.NotFound());

        if (appointment.Status != AppointmentStatus.Planned)
            return Result.Fail(AppointmentErrors.InvalidStatusForCancel());

        if (!AppointmentsTimeRules.IsCancellationAllowed(appointment, timeProvider.GetUtcNow().DateTime))
            return Result.Fail(AppointmentErrors.CancellationDeadlineExceeded);

        appointment.Cancel(command.Reason);

        await unitOfWork.SaveAsync(cancellationToken);
        return Result.Ok();
    }
}