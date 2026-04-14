using CC.AppointmentService.Application.Dependencies.UnitOfWork;

namespace CC.AppointmentService.Infrastructure.Persistence;

public class UnitOfWork(DatabaseContext db) : IUnitOfWork
{
    public Task SaveAsync(CancellationToken ct = default)
    {
        return db.SaveChangesAsync(ct);
    }
}