namespace CC.Shared.Domain.TimeRanges;

public readonly record struct DateRange
{
    public DateRange(DateOnly from, DateOnly to)
    {
        if (from >= to)
            throw new ArgumentException("Дата начала должна быть меньше даты окончания");
        From = from;
        To = to;
    }
    public DateOnly From { get; private init; }
    public DateOnly To { get; private init; }
}