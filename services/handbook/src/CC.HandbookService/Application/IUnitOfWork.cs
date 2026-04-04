using Microsoft.EntityFrameworkCore.Storage;

namespace CC.HandbookService.Application;

public interface IUnitOfWork
{
    public Task SaveAsync(CancellationToken ct = default);
    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default);
}