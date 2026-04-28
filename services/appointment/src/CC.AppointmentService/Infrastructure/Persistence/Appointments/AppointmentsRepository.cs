using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Appointments.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CC.AppointmentService.Infrastructure.Persistence.Appointments;

internal class AppointmentsRepository(DatabaseContext db) : IAppointmentsRepository
{
    public async Task AddAsync(Appointment appointment)
    {
        await db.Appointments.AddAsync(appointment);
    }

    public async Task<Appointment?> GetByIdAsync(Guid id)
    {
        return await db.Appointments.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<bool> HasInterceptsAsync(Appointment appointment)
    {
        return await db.Appointments.Where(a => a.ClientId == appointment.ClientId)
            .AnyAsync(a => a.TimeSlot.From < appointment.TimeSlot.To && a.TimeSlot.To > appointment.TimeSlot.From);
    }

    public async Task<Appointment?> GetLastAppointmentAsync(Guid clientId)
    {
        return await db.Appointments.Where(a => a.ClientId == clientId)
            .OrderByDescending(a => a.TimeSlot.To).FirstOrDefaultAsync();
    }
}