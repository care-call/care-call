using Npgsql;
using Weasel.Postgresql;

namespace CC.TechSupportService.Infrastructure.BackgroundServices.CleanUpServices;

public class TableCleanupService(
    NpgsqlDataSource dataSource,
    string sql,
    NpgsqlParameter[] parameters,
    TimeSpan cleanupInterval) : CleanupService(cleanupInterval)
{
    protected override async Task CleanupAsync(CancellationToken stoppingToken)
    {
        await using var conn = await dataSource.OpenConnectionAsync(stoppingToken);
        await using var cmd = conn.CreateCommand();
        
        cmd.CommandText = sql;
        cmd.Parameters.AddRange(parameters);
        
        await cmd.ExecuteNonQueryAsync(stoppingToken); 
    }
}