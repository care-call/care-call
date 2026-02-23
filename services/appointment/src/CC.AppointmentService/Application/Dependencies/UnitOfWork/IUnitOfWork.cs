namespace CC.AppointmentService.Application.Dependencies.UnitOfWork;

public interface IUnitOfWork
{
    Task SaveAsync(CancellationToken ct = default);
}