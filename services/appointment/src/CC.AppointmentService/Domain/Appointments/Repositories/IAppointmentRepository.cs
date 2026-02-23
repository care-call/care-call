namespace CC.AppointmentService.Domain.Appointments.Repositories;

public interface IAppointmentRepository
{
    Task AddAsync(Appointment appointment);
}