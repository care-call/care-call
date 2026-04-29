using CC.TechSupportService.Application.Interfaces;

namespace CC.TechSupportService.Infrastructure.Persistence;

public class UnitOfWork(DatabaseContext databaseContext) : IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken token)
        => databaseContext.SaveChangesAsync(token);
}