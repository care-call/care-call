using CC.TechSupportService.Domain.Abstractions;

namespace CC.TechSupportService.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly DatabaseContext _databaseContext;

    public UnitOfWork(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public Task SaveChangesAsync(CancellationToken token)
        => _databaseContext.SaveChangesAsync(token);
}