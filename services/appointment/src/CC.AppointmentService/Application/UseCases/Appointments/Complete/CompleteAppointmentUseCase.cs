using CC.AppointmentService.Application.Dependencies.UnitOfWork;
using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Appointments.Repositories;
using FluentResults;

namespace CC.AppointmentService.Application.UseCases.Appointments.Complete;

public sealed record CompleteAppointment
{
    public required Guid AppointmentId { get; init; }
    public required Guid PractitionerId { get; init; }
}

public static class CompleteAppointmentUseCase
{
    public static async ValueTask<Result> Handle(
        CompleteAppointment command,
        IUnitOfWork unitOfWork,
        IAppointmentsRepository appointmentsRepository,
        DateTime now,
        CancellationToken cancellationToken)
    {
        var appointment = await appointmentsRepository.GetByIdAsync(command.AppointmentId);
        if (appointment is null || appointment.PractitionerId != command.PractitionerId)
            return Result.Fail(AppointmentErrors.NotFound());

        var result = appointment.Complete(now);
        if (result.IsFailed)
            return result;

        await unitOfWork.SaveAsync(cancellationToken);
        return Result.Ok();
    }
}