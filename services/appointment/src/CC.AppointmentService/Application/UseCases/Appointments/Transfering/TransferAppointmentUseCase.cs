using CC.AppointmentService.Application.Dependencies.UnitOfWork;
using CC.AppointmentService.Domain;
using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Appointments.Repositories;
using CC.AppointmentService.Domain.Appointments.Rules;
using CC.Shared.Domain.TimeRanges;
using FluentResults;
using Mediator;

namespace CC.AppointmentService.Application.UseCases.Appointments.Transfering;

public record TransferAppointment : IRequest<Result>
{
    public Guid AppointmentId { get; init; }
    public DateTimeRange TimeSlot { get; init; }
}

public class TransferAppointmentUseCase(
    IAppointmentRepository appointmentRepository,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider)
    : IRequestHandler<TransferAppointment, Result>
{
    public async ValueTask<Result> Handle(TransferAppointment command, CancellationToken cancellationToken)
    {
        var appointment = await appointmentRepository.GetByIdAsync(command.AppointmentId);
        if (appointment is null)
            return Result.Fail("Заявка не существует!");

        if (appointment.Status != AppointmentStatus.Planned)
            return Result.Fail("Заявка уже завершена, перенос не возможен!");

        var currentDateTime = timeProvider.GetUtcNow();
        var daysDifference = Math.Abs((appointment.TimeSlot.From - command.TimeSlot.From).Days);

        if ((command.TimeSlot.From - currentDateTime).TotalHours < AppointmentsTimeRules.MinHoursBeforeAppointment)
        {
            return Result.Fail("Время между созданием записи и началом должно быть не меньше 4ех часов");
        }

        if (daysDifference > AppointmentsTimeRules.MaxDaysTransferAppointment)
        {
            return Result.Fail("Максимальная дальность переноса записи не больше недели от старой даты");
        }

        var hasIntersections = await appointmentRepository.HasIntercepts(command.TimeSlot);
        if (hasIntersections)
        {
            return Result.Fail("Клиент не может иметь пересекающиеся записи");
        }

        appointment.TimeSlot = command.TimeSlot;
        await unitOfWork.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}