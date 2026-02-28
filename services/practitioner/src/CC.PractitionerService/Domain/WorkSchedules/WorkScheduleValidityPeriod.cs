namespace CC.PractitionerService.Domain.WorkSchedules;

public sealed record WorkScheduleValidityPeriod
{
    public static readonly TimeSpan MinimumTimeInterval = TimeSpan.FromDays(1);

    public WorkScheduleValidityPeriod(DateOnly from, DateOnly to)
    {
        var isValidInterval = to.ToDateTime(TimeOnly.MinValue) -
                              from.ToDateTime(TimeOnly.MinValue) >=
                              MinimumTimeInterval;
        if (!isValidInterval)
            throw new ArgumentException("Допустимый минимальный интервал действия расписания нарушен");
        From = from;
        To = to;
    }

    public WorkScheduleValidityPeriod(DateOnly from)
    {
        From = from;
    }

    public DateOnly From { get; private set; }
    public DateOnly? To { get; private set; }

    public bool IsActiveOn(DateOnly date) =>
        date >= From && (!To.HasValue || date <= To.Value);
}