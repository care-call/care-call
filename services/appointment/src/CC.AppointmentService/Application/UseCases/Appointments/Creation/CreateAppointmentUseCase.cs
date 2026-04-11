using CC.AppointmentService.Application.Dependencies.BackgroundJobs;
using CC.AppointmentService.Application.Dependencies.UnitOfWork;
using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Appointments.Repositories;
using CC.Shared.Domain.TimeRanges;
using FluentResults;

namespace CC.AppointmentService.Application.UseCases.Appointments.Creation;

public sealed record CreateAppointment
{
    public required Guid ClientId { get; init; }
    public required Guid PractitionerId { get; init; }
    public required DateTimeRange TimeSlot { get; init; }
    public required ClientSnapshot ClientSnapshot { get; init; }
    public required PractitionerSnapshot PractitionerSnapshot { get; init; }
}

public class CreateAppointmentUseCase(
    IUnitOfWork unitOfWork,
    IAppointmentsRepository appointmentsRepository,
    IAppointmentBackgroundTasks jobbBackgroundTasks,
    TimeProvider timeProvider)
{
    public async ValueTask<Result> Handle(CreateAppointment command, CancellationToken cancellationToken)
    {
        var now = timeProvider.GetUtcNow().DateTime;

        var appointmentResult = Appointment.Create(
            command.ClientId,
            command.PractitionerId,
            command.TimeSlot,
            command.ClientSnapshot,
            command.PractitionerSnapshot,
            now);

        if (appointmentResult.IsFailed)
            return appointmentResult.ToResult();

        var appointment = appointmentResult.Value;

        var lastAppointment = await appointmentsRepository.GetLastAppointmentAsync(command.ClientId);
        if (lastAppointment is not null && appointment.HasInsufficientBreakAfter(lastAppointment))
            return Result.Fail(AppointmentErrors.MinBreakBetweenAppointments);

        var hasIntercept = await appointmentsRepository.HasInterceptsAsync(appointment);
        if (hasIntercept)
            return Result.Fail(AppointmentErrors.HasIntercepts());

        await appointmentsRepository.AddAsync(appointment);
        await unitOfWork.SaveAsync(cancellationToken);
        await jobbBackgroundTasks.ScheduleCallCreationAsync(appointment);
        return Result.Ok();
    }
}
