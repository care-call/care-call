using CC.AppointmentService.Application.Dependencies.UnitOfWork;
using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Appointments.Repositories;
using CC.Shared.Domain.TimeRanges;
using FluentResults;

namespace CC.AppointmentService.Application.UseCases.Appointments.Transferring;

public sealed record TransferAppointment
{
    public required Guid AppointmentId { get; init; }
    public required DateTimeRange TimeSlot { get; init; }
    public required Guid ClientId { get; init; }
}

public static class TransferAppointmentUseCase
{
    public static async ValueTask<Result> Handle(TransferAppointment command,
        IAppointmentsRepository appointmentsRepository,
        IUnitOfWork unitOfWork,
        DateTime now,
        CancellationToken cancellationToken)
    {
        var appointment = await appointmentsRepository.GetByIdAsync(command.AppointmentId);
        if (appointment is null || appointment.ClientId != command.ClientId)
            return Result.Fail(AppointmentErrors.NotFound());
        
        var lastAppointment = await appointmentsRepository.GetLastAppointmentAsync(command.ClientId);
        if (lastAppointment is not null && appointment.HasInsufficientBreakAfter(lastAppointment))
            return Result.Fail(AppointmentErrors.MinBreakBetweenAppointments);

        var result = appointment.Transfer(command.TimeSlot, now);
        if (result.IsFailed)
            return result;

        var hasIntersections = await appointmentsRepository.HasInterceptsAsync(appointment);
        if (hasIntersections)
            return Result.Fail(AppointmentErrors.HasIntercepts());

        await unitOfWork.SaveAsync(cancellationToken);
        return Result.Ok();
    }
}
