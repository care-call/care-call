using CC.HandbookService.Application;
using Microsoft.EntityFrameworkCore.Storage;

namespace CC.HandbookService.Infrastructure.Persistence;

public class UnitOfWork(DatabaseContext db) : IUnitOfWork
{
    public Task SaveAsync(CancellationToken ct = default) 
        => db.SaveChangesAsync(ct);
    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default)
        => db.Database.BeginTransactionAsync(ct);
}