using CC.HandbookService.Application;

namespace CC.HandbookService.Infrastructure;

public class UnitOfWork(DatabaseContext db) : IUnitOfWork
{
    public Task SaveAsync(CancellationToken ct = default) => db.SaveChangesAsync(ct);
}