using CC.Shared.Domain.TimeRanges;

namespace CC.AppointmentService.Domain.Appointments.Repositories;

public interface IAppointmentsRepository
{
    Task AddAsync(Appointment appointment);
    Task<Appointment?> GetByIdAsync(Guid id);
    Task<bool> HasInterceptsAsync(DateTimeRange range, Guid clientId);
    Task<Appointment?> GetLastAppointmentAsync(Guid clientId);
}