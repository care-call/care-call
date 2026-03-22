using CC.AppointmentService.Application.Dependencies.BackgroundJobs;
using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Infrastructure.BackgroundJobs.Jobs;
using Hangfire;

namespace CC.AppointmentService.Infrastructure.BackgroundJobs;

public class AppointmentBackgroundTasks(
    IBackgroundJobClient jobClient,
    CallCreationJob job) : IAppointmentBackgroundTasks
{
    public Task ScheduleCallCreationAsync(Appointment appointment)
    {
        jobClient.Enqueue(() => job.Execute(appointment.Id, CancellationToken.None));
        return Task.CompletedTask;
    }
}