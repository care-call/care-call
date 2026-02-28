namespace CC.AppointmentService.Domain.Appointments.Repositories;

public interface IAppointmentRepository
{
    Task AddAsync(Appointment appointment);
    Task<ICollection<Appointment?>> GetAllAsync();
    Task<ICollection<Appointment?>> GetByClientIdAsync(Guid clientId);
    Task<Appointment?> GetByIdAsync(Guid id);
}