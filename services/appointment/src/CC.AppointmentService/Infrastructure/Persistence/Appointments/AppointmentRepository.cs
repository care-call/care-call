using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Appointments.Repositories;

namespace CC.AppointmentService.Infrastructure.Persistence.Appointments;

internal class AppointmentRepository(DatabaseContext db) : IAppointmentRepository
{
    public async Task AddAsync(Appointment appointment) =>
        await db.Appointments.AddAsync(appointment);
}