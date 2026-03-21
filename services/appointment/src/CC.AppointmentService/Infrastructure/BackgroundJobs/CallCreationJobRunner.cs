using CC.AppointmentService.Application.Dependencies.BackgroundJobs;
using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Infrastructure.BackgroundJobs.Jobs;
using Hangfire;

namespace CC.AppointmentService.Infrastructure.BackgroundJobs;

public class CallCreationJobRunner(
    IBackgroundJobClient jobClient,
    CallCreationJob job) : ICallCreationJobRunner
{
    public Task RunAsync(Appointment appointment)
    {
        jobClient.Enqueue(() =>  job.Execute(appointment.Id));
        return Task.CompletedTask;
    }
}