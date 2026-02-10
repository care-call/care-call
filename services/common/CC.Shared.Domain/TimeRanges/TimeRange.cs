namespace CC.Shared.Domain.TimeRanges;

public readonly record struct TimeRange
{
    public TimeRange(TimeOnly from, TimeOnly to)
    {
        if (from == to)
            throw new ArgumentException("Временной промежуток не может быть нулевой длины");
        From = from;
        To = to;
    }

    public TimeOnly From { get; private init; }
    public TimeOnly To { get; private init; }
    public bool CrossesMidnight => From < To;

    public bool Contains(TimeOnly now) =>
        !CrossesMidnight
            ? From <= now && now <= To
            : now >= From || now <= To;
    
    public static IReadOnlyList<TimeRange> Subtract(TimeRange source, TimeRange toRemove)
    {
        if (toRemove.To <= source.From || toRemove.From >= source.To)
            return [source];

        var result = new List<TimeRange>();

        if (source.From < toRemove.From)
            result.Add(new TimeRange(source.From, toRemove.From));

        if (toRemove.To < source.To)
            result.Add(new TimeRange(toRemove.To, source.To));

        return result;
    }

    public TimeOnly Add(TimeSpan span) => TimeOnly.FromTimeSpan(From.ToTimeSpan().Add(span));
    public TimeOnly Max(TimeOnly other) => From > other ? From : other;

    public bool Intersects(TimeRange other)
    {
        if (!CrossesMidnight && !other.CrossesMidnight)
            return From < other.To && other.From < To;

        return Contains(other.From) || 
               Contains(other.To) || 
               other.Contains(From) || 
               other.Contains(To);
    }
}