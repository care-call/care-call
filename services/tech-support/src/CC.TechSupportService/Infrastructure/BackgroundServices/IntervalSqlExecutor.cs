using Npgsql;

namespace CC.TechSupportService.Infrastructure.BackgroundServices;

public class IntervalSqlExecutor(
    NpgsqlDataSource dataSource,
    string sql,
    NpgsqlParameter[] parameters,
    TimeSpan interval,
    ILogger logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(interval, stoppingToken);

            try
            {
                await using var conn = await dataSource.OpenConnectionAsync(stoppingToken);
                await using var cmd = conn.CreateCommand();
                
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(parameters);
                
                await cmd.ExecuteNonQueryAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при выполнении SQL запроса");
            }
        }
    }
}