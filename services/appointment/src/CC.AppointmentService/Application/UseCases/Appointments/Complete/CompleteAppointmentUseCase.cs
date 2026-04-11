using CC.AppointmentService.Application.Dependencies.UnitOfWork;
using CC.AppointmentService.Domain.Appointments.Repositories;
using CC.AppointmentService.Domain.Errors;
using FluentResults;

namespace CC.AppointmentService.Application.UseCases.Appointments.Complete;

public sealed record CompleteAppointment
{
    public required Guid AppointmentId { get; init; }
    public required Guid PractitionerId { get; init; }
}

public class CompleteAppointmentUseCase(
    IUnitOfWork unitOfWork,
    IAppointmentsRepository appointmentsRepository,
    TimeProvider timeProvider)
{
    public async ValueTask<Result> Handle(CompleteAppointment command, CancellationToken cancellationToken)
    {
        var appointment = await appointmentsRepository.GetByIdAsync(command.AppointmentId);
        if (appointment is null || appointment.PractitionerId != command.PractitionerId)
            return Result.Fail(AppointmentErrors.NotFound());

        var result = appointment.Complete(timeProvider.GetUtcNow().UtcDateTime);
        if (result.IsFailed)
            return result;

        await unitOfWork.SaveAsync(cancellationToken);
        return Result.Ok();
    }
}
