using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Appointments.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CC.AppointmentService.Infrastructure.Persistence.Appointments;

internal class AppointmentsRepository(DatabaseContext db) : IAppointmentsRepository
{
    public void Add(Appointment appointment)
        => db.Appointments.Add(appointment);

    public Task<Appointment?> GetByIdAsync(Guid id)
        => db.Appointments.FirstOrDefaultAsync(a => a.Id == id);

    public Task<bool> HasInterceptsAsync(Appointment appointment) 
        => db.Appointments.Where(a => a.ClientId == appointment.ClientId)
            .AnyAsync(a => a.TimeSlot.From < appointment.TimeSlot.To && a.TimeSlot.To > appointment.TimeSlot.From);

    public Task<Appointment?> GetLastAppointmentAsync(Guid clientId)
        => db.Appointments.Where(a => a.ClientId == clientId)
            .OrderByDescending(a => a.TimeSlot.To).FirstOrDefaultAsync();
}