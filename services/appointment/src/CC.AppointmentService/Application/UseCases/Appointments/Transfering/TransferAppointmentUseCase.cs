using CC.AppointmentService.Application.Dependencies.UnitOfWork;
using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Appointments.Repositories;
using CC.Shared.Domain.TimeRanges;
using FluentResults;
using Mediator;

namespace CC.AppointmentService.Application.UseCases.Appointments.Rescheduling;

public record TransferAppointment : IRequest<Result>
{
    public Guid AppointmentId { get; init; }
    public DateTimeRange TimeSlot { get; init; }
}

public class TransferAppointmentUseCase(
    IAppointmentRepository appointmentRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<TransferAppointment, Result>
{
    public async ValueTask<Result> Handle(TransferAppointment command, CancellationToken cancellationToken)
    {
        var appointment = await appointmentRepository.GetByIdAsync(command.AppointmentId);
        if (appointment is null)
            return Result.Fail("Заявка не существует!");

        if (appointment.Status != AppointmentStatus.Planned)
            return Result.Fail("Заявка уже завершена, перенос не возможен!");

        var currentDateTime = DateTime.UtcNow;
        var daysDifference = Math.Abs((appointment.TimeSlot.From - command.TimeSlot.From).Days);
        var minHours = 4;
        var minDays = 7;

        if ((command.TimeSlot.From - currentDateTime).TotalHours < minHours)
        {
            return Result.Fail("Время между созданием записи и началом должно быть не меньше 4ех часов");
        }

        if (daysDifference > minDays)
        {
            return Result.Fail("Максимальная дальность переноса записи не больше недели от старой даты");
        }

        var appointments = await appointmentRepository.GetAllAsync();
        var hasIntersections = appointments.Any(a => 
            a.ClientId == appointment.ClientId
            && a.Id != appointment.Id
            && a.TimeSlot.From < command.TimeSlot.To
            && a.TimeSlot.To > command.TimeSlot.From);

        if (hasIntersections)
        {
            return Result.Fail("Клиент не может иметь пересекающиеся записи");
        }

        appointment.TimeSlot = command.TimeSlot;
        await unitOfWork.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}