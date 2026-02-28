using CC.AppointmentService.Application.Dependencies.UnitOfWork;
using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Appointments.Repositories;
using CC.Shared.Domain.TimeRanges;
using FluentResults;
using Mediator;

namespace CC.AppointmentService.Application.UseCases.Appointments.Creation;

public record CreateAppointment : IRequest<Result>
{
    public required Guid ClientId { get; init; }
    public required Guid PractitionerId { get; init; }
    public required DateTimeRange TimeSlot { get; init; }
    public required ClientSnapshot ClientSnapshot { get; init; }
    public required PractitionerSnapshot PractitionerSnapshot { get; init; }
}

public class CreateAppointmentUseCase(
    IUnitOfWork unitOfWork,
    IAppointmentRepository appointmentRepository) : IRequestHandler<CreateAppointment, Result>
{
    public async ValueTask<Result> Handle(CreateAppointment command, CancellationToken cancellationToken)
    {
        var currentDateTime = DateTime.UtcNow;
        const int minHours = 4;
        const int minMinutes = 30;
        
        if ((command.TimeSlot.From - currentDateTime).TotalHours < minHours)
        {
            return Result.Fail("Время между созданием записи и началом должно быть не меньше 4 часов");
        }

        var clientAppointments = await appointmentRepository.GetByClientIdAsync(command.ClientId);

        if (clientAppointments.Count != 0)
        {
            var hasIntersections = clientAppointments.Any(a =>
                a.TimeSlot.From < command.TimeSlot.To &&
                a.TimeSlot.To > command.TimeSlot.From
            );

            if (hasIntersections)
            {
                return Result.Fail("Клиент не может иметь пересекающиеся записи");
            }

            var lastApp = clientAppointments
                .OrderByDescending(a => a.TimeSlot.To)
                .FirstOrDefault();

            if (lastApp != null && (command.TimeSlot.From - lastApp.TimeSlot.To).TotalMinutes < minMinutes)
            {
                return Result.Fail("Минимальный интервал между записями — 30 минут");
            }
        }

        var appointment = new Appointment(Guid.CreateVersion7())
        {
            ClientId = command.ClientId,
            PractitionerId = command.PractitionerId,
            TimeSlot = command.TimeSlot,
            Status = AppointmentStatus.Planned,
            ClientSnapshot = command.ClientSnapshot,
            PractitionerSnapshot = command.PractitionerSnapshot
        };

        await appointmentRepository.AddAsync(appointment);
        await unitOfWork.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}