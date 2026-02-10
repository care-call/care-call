namespace CC.Shared.Domain.TimeRanges;

public readonly record struct DateTimeRange
{
    public DateTimeRange(DateTime from, DateTime to)
    {
        if (from >= to)
            throw new ArgumentException("Дата и время начала должна быть меньше даты и времени окончания");
        From = from;
        To = to;
    }
    public DateTime From { get; private init; }
    public DateTime To { get; private init; }
}