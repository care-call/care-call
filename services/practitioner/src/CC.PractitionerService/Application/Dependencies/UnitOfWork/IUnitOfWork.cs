namespace CC.PractitionerService.Application.Dependencies.UnitOfWork;

public interface IUnitOfWork
{
    Task SaveAsync(CancellationToken ct = default);
}