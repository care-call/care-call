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

public class TransferAppointmentUseCase(
    IAppointmentsRepository appointmentsRepository,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider)
{
    public async ValueTask<Result> Handle(TransferAppointment command, CancellationToken cancellationToken)
    {
        var appointment = await appointmentsRepository.GetByIdAsync(command.AppointmentId);
        if (appointment is null || appointment.ClientId != command.ClientId)
            return Result.Fail(AppointmentErrors.NotFound());

        var result = appointment.Transfer(command.TimeSlot, timeProvider.GetUtcNow().DateTime);
        if (result.IsFailed)
            return result;

        // Проверка пересечений после применения нового слота,
        // но до сохранения — откат происходит неявно через отказ от SaveAsync.
        var hasIntersections = await appointmentsRepository.HasInterceptsAsync(appointment);
        if (hasIntersections)
            return Result.Fail(AppointmentErrors.HasIntercepts());

        await unitOfWork.SaveAsync(cancellationToken);
        return Result.Ok();
    }
}
