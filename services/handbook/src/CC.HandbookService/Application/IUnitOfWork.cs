namespace CC.HandbookService.Application;

public interface IUnitOfWork
{
    public Task SaveAsync(CancellationToken ct = default);
}