using CC.AppointmentService.Application.Dependencies.UnitOfWork;
using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Appointments.Repositories;
using FluentResults;

namespace CC.AppointmentService.Application.UseCases.Appointments.Cancellation;

public sealed record CancelAppointment
{
    public required Guid AppointmentId { get; init; }
    public required Guid ClientId { get; init; }
    public required string Reason { get; init; }
}

public static class CancelAppointmentUseCase
{
    public static async ValueTask<Result> Handle(
        CancelAppointment command,
        IUnitOfWork unitOfWork,
        IAppointmentsRepository appointmentsRepository,
        DateTime now,
        CancellationToken cancellationToken)
    {
        var appointment = await appointmentsRepository.GetByIdAsync(command.AppointmentId);
        if (appointment is null || appointment.ClientId != command.ClientId)
            return Result.Fail(AppointmentErrors.NotFound());

        var result = appointment.Cancel(CancellationReason.From(command.Reason), now);
        if (result.IsFailed)
            return result;

        await unitOfWork.SaveAsync(cancellationToken);
        return Result.Ok();
    }
}
