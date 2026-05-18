using CC.AppointmentService.Application.Dependencies.UnitOfWork;
using Wolverine.EntityFrameworkCore;

namespace CC.AppointmentService.Infrastructure.Persistence;

public class UnitOfWork(IDbContextOutbox<DatabaseContext> outbox) : IUnitOfWork
{
    public Task SaveAsync(CancellationToken ct = default)
    {
        return outbox.SaveChangesAndFlushMessagesAsync(ct);
    }
}