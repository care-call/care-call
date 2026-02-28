using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Appointments.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CC.AppointmentService.Infrastructure.Persistence.Appointments;

internal class AppointmentRepository(DatabaseContext db) : IAppointmentRepository
{
    public async Task AddAsync(Appointment appointment) =>
        await db.Appointments.AddAsync(appointment);

    public async Task<ICollection<Appointment>> GetAllAsync() =>
        await db.Appointments.ToListAsync();

    public async Task<ICollection<Appointment?>> GetByClientIdAsync(Guid clientId) =>
        await db.Appointments.Where(a => a.ClientId == clientId).ToListAsync();

    public async Task<Appointment?> GetByIdAsync(Guid id) =>
        await db.Appointments.FirstOrDefaultAsync(a => a.Id == id);
}