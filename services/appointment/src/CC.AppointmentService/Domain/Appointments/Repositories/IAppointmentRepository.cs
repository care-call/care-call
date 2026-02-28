using CC.Shared.Domain.TimeRanges;

namespace CC.AppointmentService.Domain.Appointments.Repositories;

public interface IAppointmentRepository
{
    Task AddAsync(Appointment appointment);
    Task<ICollection<Appointment?>> GetAllAsync();
    Task<ICollection<Appointment?>> GetByClientIdAsync(Guid clientId);
    Task<Appointment?> GetByIdAsync(Guid id);
    Task<bool> HasIntercepts(DateTimeRange range);
    Task<Appointment?> GetLastAppointment(Guid clientId);
}