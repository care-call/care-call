namespace CC.Shared.Domain.TimeRanges;

public sealed record Week
{
    public Week(DateTime start)
    {
        StartedAt = start.DayOfWeek == DayOfWeek.Monday ? start : throw new ArgumentOutOfRangeException(nameof(start));
        EndedAt = start.AddDays(7);
    }
    public DateTime StartedAt { get; init; }
    public DateTime EndedAt { get; init; }
    
    public static explicit operator DateTimeRange(Week week) => new(week.StartedAt, week.EndedAt);

    public bool IsInclusionPeriod(DateTimeRange period) => period.From >= StartedAt && period.To <= EndedAt;
}