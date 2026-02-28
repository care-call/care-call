using CC.Shared.Domain.TimeRanges;

namespace CC.AppointmentService.Domain.Appointments.Repositories;

public interface IAppointmentsRepository
{
    Task AddAsync(Appointment appointment);
    Task<Appointment?> GetByIdAsync(Guid id);
    Task<bool> HasIntercepts(DateTimeRange range, Guid clientId);
    Task<Appointment?> GetLastAppointment(Guid clientId);
}