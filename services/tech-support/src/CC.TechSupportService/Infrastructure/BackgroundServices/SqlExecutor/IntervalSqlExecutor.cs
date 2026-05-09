using Npgsql;

namespace CC.TechSupportService.Infrastructure.BackgroundServices.SqlExecutor;

public class IntervalSqlExecutor: BackgroundService
{
    private readonly NpgsqlDataSource _dataSource;
    private readonly string _sql;
    private readonly NpgsqlParameter[] _parameters;
    private readonly ILogger _logger;
    private readonly TimeSpan _interval;

    private TimeSpan _currInterval;
    private readonly TimeSpan _retryInterval = TimeSpan.FromMinutes(1);
    
    public IntervalSqlExecutor(
        NpgsqlDataSource dataSource,
        string sql,
        NpgsqlParameter[] parameters,
        TimeSpan interval,
        ILogger logger)
    {
        _dataSource = dataSource;
        _sql = sql;
        _parameters = parameters;
        _interval = interval;
        _currInterval = interval;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await using var conn = await _dataSource.OpenConnectionAsync(stoppingToken);
                await using var cmd = conn.CreateCommand();
                
                cmd.CommandText = _sql;
                cmd.Parameters.AddRange(_parameters);
                
                await cmd.ExecuteNonQueryAsync(stoppingToken);
                
                _currInterval = _interval;
            }
            catch (Exception ex)
            {
                _logger.SqlExecutionFailed(ex);
                
                _currInterval = _retryInterval;
            }
            
            await Task.Delay(_currInterval, stoppingToken);
        }
    }
}