using CC.AppointmentService.Domain.Appointments;

namespace CC.AppointmentService.Application.Dependencies.BackgroundJobs;

public interface ICallCreationJobRunner
{
    public Task RunAsync(Appointment appointment);
}