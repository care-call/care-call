using CC.Shared.Domain.TimeRanges;

namespace CC.PractitionerService.Domain.WorkSchedules;

public record WeeklyRecurrence
{
    private WeeklyRecurrence()
    {
    }

    public WeeklyRecurrence(DayOfWeek day, TimeRange timeRange)
    {
        Day = day;
        TimeRange = timeRange;
    }

    public DayOfWeek Day { get; init; }
    public TimeRange TimeRange { get; init; }
}