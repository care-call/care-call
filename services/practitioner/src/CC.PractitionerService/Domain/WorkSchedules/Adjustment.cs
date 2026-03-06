using CC.Shared.Domain;
using CC.Shared.Domain.TimeRanges;

namespace CC.PractitionerService.Domain.WorkSchedules;

/// <summary>
/// Корректировка рабочего графика.
/// </summary>
public sealed class Adjustment : Entity<Guid>
{
    public Adjustment(
        Guid id,
        Guid workScheduleId,
        AdjustmentType type,
        DateTimeRange period) : this(id)
    {
        WorkScheduleId = workScheduleId;
        Type = type;
        Period = period;
    }

#pragma warning disable CS8618 // Для EF core.
    private Adjustment(Guid id) : base(id)
    {
    }

    public Guid WorkScheduleId { get; set; }
    public DateTimeRange Period { get; set; }
    public AdjustmentType Type { get; set; }
    public string? Description { get; set; }

    public bool Intersects(Adjustment adjustment)
        => adjustment.Period.From < Period.To && Period.From < adjustment.Period.To;
}