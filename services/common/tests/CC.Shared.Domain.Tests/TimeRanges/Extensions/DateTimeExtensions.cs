using CC.Shared.Domain.TimeRanges;

namespace CC.Shared.Domain.Tests.TimeRanges.Extensions;

public static class DateTimeRangeExtensions
{
    extension(DateTimeRange)
    {
        public static DateTimeRange Range(string timeFrom, string timeTo, DateTime? baseDate = null)
        {
            baseDate ??= new DateTime(2026, 1, 1);
            return new DateTimeRange(
                DateTime.Parse($"{baseDate:yyyy-MM-dd} {timeFrom}"),
                DateTime.Parse($"{baseDate:yyyy-MM-dd} {timeTo}"));
        }
    }
}
