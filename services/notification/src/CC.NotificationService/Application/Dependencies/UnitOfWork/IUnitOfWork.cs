namespace CC.NotificationService.Application.Dependencies.UnitOfWork;

public interface IUnitOfWork
{
    Task SaveAsync(CancellationToken ct = default);
}