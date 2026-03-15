using CC.NotificationService.Application.Dependencies.UnitOfWork;

namespace CC.NotificationService.Infrastructure.Persistence;

public class UnitOfWork(DatabaseContext db) : IUnitOfWork
{
    public Task SaveAsync(CancellationToken ct = default) => db.SaveChangesAsync(ct);
}