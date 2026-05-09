namespace CC.TechSupportService.Infrastructure.BackgroundServices.SqlExecutor;

public static partial class IntervalSqlExecutorLogs
{
    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Ошибка при выполнении SQL запроса")]
    public static partial void SqlExecutionFailed(this ILogger logger, Exception ex);
}