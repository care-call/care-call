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

public static class CreateAppointmentUseCase
{
    public static async ValueTask<Result> Handle(
        CreateAppointment command,
        IUnitOfWork unitOfWork,
        IAppointmentsRepository appointmentsRepository,
        IAppointmentBackgroundTasks jobBackgroundTasks,
        DateTime now,
        CancellationToken cancellationToken)
    {
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

        appointmentsRepository.Add(appointment);
        await unitOfWork.SaveAsync(cancellationToken);
        await jobBackgroundTasks.ScheduleCallCreationAsync(appointment);
        return Result.Ok();
    }
}