using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Appointments.Repositories;
using CC.Shared.Domain.TimeRanges;
using Microsoft.EntityFrameworkCore;

namespace CC.AppointmentService.Infrastructure.Persistence.Appointments;

internal class AppointmentsRepository(DatabaseContext db) : IAppointmentRepository
{
    public async Task AddAsync(Appointment appointment) =>
        await db.Appointments.AddAsync(appointment);

    public async Task<ICollection<Appointment>> GetAllAsync() =>
        await db.Appointments.ToListAsync();

    public async Task<ICollection<Appointment?>> GetByClientIdAsync(Guid clientId) =>
        await db.Appointments.Where(a => a.ClientId == clientId).ToListAsync();

    public async Task<Appointment?> GetByIdAsync(Guid id) =>
        await db.Appointments.FirstOrDefaultAsync(a => a.Id == id);

    public async Task<bool> HasIntercepts(DateTimeRange range) =>
        await db.Appointments.AnyAsync(a => a.TimeSlot.From < range.To && a.TimeSlot.To > range.From);

    public async Task<Appointment?> GetLastAppointment(Guid clientId) =>
        await db.Appointments.Where(a => a.ClientId == clientId)
            .OrderByDescending(a => a.TimeSlot.To).FirstOrDefaultAsync();
}