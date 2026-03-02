using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Appointments.Repositories;
using CC.Shared.Domain.TimeRanges;
using Microsoft.EntityFrameworkCore;

namespace CC.AppointmentService.Infrastructure.Persistence.Appointments;

internal class AppointmentsRepository(DatabaseContext db) : IAppointmentsRepository
{
    public async Task AddAsync(Appointment appointment) =>
        await db.Appointments.AddAsync(appointment);

    public async Task<Appointment?> GetByIdAsync(Guid id, Guid clientId) =>
        await db.Appointments.Where(a => a.ClientId == clientId).FirstOrDefaultAsync(a => a.Id == id);

    public async Task<bool> HasInterceptsAsync(DateTimeRange range, Guid clientId) =>
        await db.Appointments.Where(a => a.ClientId == clientId)
            .AnyAsync(a => a.TimeSlot.From < range.To && a.TimeSlot.To > range.From);

    public async Task<Appointment?> GetLastAppointmentAsync(Guid clientId) =>
        await db.Appointments.Where(a => a.ClientId == clientId)
            .OrderByDescending(a => a.TimeSlot.To).FirstOrDefaultAsync();
}