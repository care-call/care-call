namespace CC.Shared.Domain.TimeRanges;

public sealed record Week
{
    public Week(DateTime start)
    {
        StartedAt = start.DayOfWeek == DayOfWeek.Monday ? start : throw new ArgumentOutOfRangeException(nameof(start));
    }

    public DateTime StartedAt { get; init; }
    public DateTime EndedAt => StartedAt.AddDays(7);
}