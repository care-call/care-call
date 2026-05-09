using Npgsql;

namespace CC.TechSupportService.Infrastructure.BackgroundServices.SqlExecutor;

public class IntervalSqlExecutor(
    NpgsqlDataSource dataSource,
    string sql,
    NpgsqlParameter[] parameters,
    TimeSpan interval,
    ILogger logger) : BackgroundService
{
    private readonly TimeSpan _retryInterval = TimeSpan.FromMinutes(1);
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await using var conn = await dataSource.OpenConnectionAsync(stoppingToken);
                await using var cmd = conn.CreateCommand();
                
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(parameters);
                
                await cmd.ExecuteNonQueryAsync(stoppingToken);
                
                await Task.Delay(interval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                logger.SqlExecutionFailed(ex);
                
                await Task.Delay(_retryInterval, stoppingToken);
            }
        }
    }
}