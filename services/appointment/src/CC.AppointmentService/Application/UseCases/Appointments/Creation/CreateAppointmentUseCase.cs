using CC.AppointmentService.Application.Dependencies.UnitOfWork;
using CC.AppointmentService.Domain;
using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Appointments.Repositories;
using CC.AppointmentService.Domain.Appointments.Rules;
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
    IAppointmentRepository appointmentRepository,
    TimeProvider timeProvider) : IRequestHandler<CreateAppointment, Result>
{
    public async ValueTask<Result> Handle(CreateAppointment command, CancellationToken cancellationToken)
    {
        var currentDateTime = timeProvider.GetUtcNow();
        if ((command.TimeSlot.From - currentDateTime).TotalHours < AppointmentsTimeRules.MinHoursBeforeAppointment)
        {
            return Result.Fail("Время между созданием записи и началом должно быть не меньше 4 часов");
        }

        var hasIntercept = await appointmentRepository.HasIntercepts(command.TimeSlot);

        if (hasIntercept)
        {
            return Result.Fail("Клиент не может иметь пересекающиеся записи");
        }

        var lastAppointment = await appointmentRepository.GetLastAppointment(command.ClientId);
        if (lastAppointment != null && (command.TimeSlot.From - lastAppointment.TimeSlot.To).TotalMinutes <
            AppointmentsTimeRules.MinMinutesBetweenAppointments)
        {
            return Result.Fail("Минимальный интервал между записями — 30 минут");
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