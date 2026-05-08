namespace CC.TechSupportService.Infrastructure.BackgroundServices.CleanUpServices;

public abstract class CleanupService(TimeSpan cleanupInterval) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(cleanupInterval, stoppingToken);
            await CleanupAsync(stoppingToken);
        }
    }

    protected abstract Task CleanupAsync(CancellationToken stoppingToken);
}