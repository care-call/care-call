using CC.AppointmentService.Application.Dependencies.UnitOfWork;
using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Appointments.Repositories;
using CC.AppointmentService.Domain.Errors;
using FluentResults;
using Mediator;

namespace CC.AppointmentService.Application.UseCases.Appointments.Complete;

public sealed record CompleteAppointment : IRequest<Result>
{
    public required Guid AppointmentId { get; init; }
    public required Guid Practicant { get; init; }
}


public class CompleteAppointmentUseCase(
    IUnitOfWork unitOfWork,
    IAppointmentsRepository appointmentsRepository
    ) : IRequestHandler<CompleteAppointment, Result>
{
    public async ValueTask<Result> Handle(CompleteAppointment command, CancellationToken cancellationToken)
    {
        var appointment = await appointmentsRepository.GetByIdAsync(command.AppointmentId);
        if(appointment ==null || appointment.PractitionerId != command.Practicant)
            return Result.Fail(AppointmentErrors.NotFound());
        if(appointment.Status != AppointmentStatus.InProgress)
            return Result.Fail(AppointmentErrors.InvalidStatusForComplete());
        appointment.Complete(DateTime.UtcNow);
        
        await unitOfWork.SaveAsync(cancellationToken);
        return Result.Ok();
    }
}