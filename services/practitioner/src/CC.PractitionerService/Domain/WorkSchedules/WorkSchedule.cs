using CC.Shared.Domain;
using CC.Shared.Domain.Exceptions;

namespace CC.PractitionerService.Domain.WorkSchedules;

/// <summary>
/// График работы.
/// </summary>
public sealed class WorkSchedule : AggregationRoot<Guid>
{
    private readonly string _timeZoneId;
    public WorkSchedule(Guid id,
        Guid practitionerId,
        WorkScheduleValidityPeriod validityPeriod,
        TimeZoneInfo timeZone,
        List<WeeklyRecurrence> recurrences) : this(id)
    {
        PractitionerId = practitionerId;
        ValidityPeriod = validityPeriod;
        Recurrences = recurrences;
        _timeZoneId = timeZone.Id;
    }

    #pragma warning disable CS8618 // Для EF core.
    private WorkSchedule(Guid id) : base(id)
    {
    }

    public Guid PractitionerId { get; private set; }

    /// <summary>
    /// Временной диапазон действия расписания.
    /// </summary>
    public WorkScheduleValidityPeriod ValidityPeriod { get; private set; }
    public TimeZoneInfo TimeZone => TimeZoneInfo.FindSystemTimeZoneById(_timeZoneId);
    public List<WeeklyRecurrence> Recurrences { get; private set; }

    /// <summary>
    /// Длительность стандартной сессии.
    /// </summary>
    public SessionDuration SessionDuration { get; set; } = SessionDuration.Default;

    public WorkScheduleStatus Status { get; set; } = WorkScheduleStatus.Draft;

    public void EndOn(DateOnly date)
    {
        if (ValidityPeriod.To.HasValue)
            throw new DomainException("График работы уже закрыт");
        var endDate = date.AddDays(-1);
        ValidityPeriod = new WorkScheduleValidityPeriod(ValidityPeriod.From, endDate);
    }

    public bool IsActiveOn(DateOnly queryDate)
        => ValidityPeriod.IsActiveOn(queryDate);
}