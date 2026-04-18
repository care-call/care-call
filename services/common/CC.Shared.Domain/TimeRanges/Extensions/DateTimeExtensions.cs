namespace CC.Shared.Domain.TimeRanges.Extensions;

public static class DateTimeExtensions
{
    extension(DateTime)
    {
        public static DateTime Max(DateTime a, DateTime b) => a > b ? a : b;
        public static DateTime Min(DateTime a, DateTime b) => a < b ? a : b;
    }
}
