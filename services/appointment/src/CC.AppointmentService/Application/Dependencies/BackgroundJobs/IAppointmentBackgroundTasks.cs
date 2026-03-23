using CC.AppointmentService.Domain.Appointments;

namespace CC.AppointmentService.Application.Dependencies.BackgroundJobs;

public interface IAppointmentBackgroundTasks
{
    public Task ScheduleCallCreationAsync(Appointment appointment);
}