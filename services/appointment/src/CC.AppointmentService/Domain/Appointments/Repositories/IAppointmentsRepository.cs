namespace CC.AppointmentService.Domain.Appointments.Repositories;

public interface IAppointmentsRepository
{
    Task AddAsync(Appointment appointment);
    Task<Appointment?> GetByIdAsync(Guid id);
    Task<bool> HasInterceptsAsync(Appointment appointment);
    Task<Appointment?> GetLastAppointmentAsync(Guid clientId);
}