using CC.PractitionerService.Domain.WorkSchedules;
using CC.Shared.Domain.TimeRanges;
using CC.Shared.Domain.TimeRanges.Extensions;

namespace CC.PractitionerService.Domain.Availability.Services;

public sealed class PractitionerAvailabilityCalculator
{
    public IReadOnlyCollection<DateTimeRange> Calculate(
        IEnumerable<WorkSchedule> workSchedules,
        IEnumerable<Adjustment> adjustments,
        IReadOnlyCollection<EmploymentSnapshot> employments,
        DateTimeRange period)
    {
        var schedules = GenerateSchedules(workSchedules, period);
        var adjustedSchedules = ApplyAdjustments(schedules, adjustments, period);
        return ApplyEmployments(adjustedSchedules, employments, period);
    }

    private static IReadOnlyCollection<DateTimeRange> GenerateSchedules(
        IEnumerable<WorkSchedule> workSchedules,
        DateTimeRange period)
    {
        var slots = new List<DateTimeRange>();
        var periodStart = DateOnly.FromDateTime(period.From);
        var periodEnd = DateOnly.FromDateTime(period.To);
        
        foreach (var schedule in workSchedules.Where(s => s.ValidityPeriod.HasOverlapWith(period)))
        {
            var scheduleEnd = schedule.ValidityPeriod.To ?? periodEnd;
            var effectiveEnd = scheduleEnd < periodEnd ? scheduleEnd : periodEnd;

            for (var date = periodStart; date <= effectiveEnd; date = date.AddDays(1))
            {
                foreach (var recurrence in schedule.Recurrences.Where(r => r.Day == date.DayOfWeek))
                {
                    var slotStart = date.ToDateTime(recurrence.TimeRange.From);
                    var slotEnd = date.ToDateTime(recurrence.TimeRange.To);

                    var clippedStart = DateTime.Max(slotStart, period.From);
                    var clippedEnd = DateTime.Min(slotEnd, period.To);

                    if (clippedStart < clippedEnd)
                        slots.Add(new DateTimeRange(clippedStart, clippedEnd));
                }
            }
        }
        
        return slots.MergeRanges();
    }

    private static IReadOnlyCollection<DateTimeRange> ApplyAdjustments(
        IReadOnlyCollection<DateTimeRange> slots,
        IEnumerable<Adjustment> adjustments,
        DateTimeRange period)
    {
        IEnumerable<DateTimeRange> result = slots;

        var relevantAdjustments = adjustments
            .Where(a => a.Period.From < period.To && a.Period.To > period.From)
            .OrderBy(a => a.Period.From);

        foreach (var adjustment in relevantAdjustments)
        {
            var adjStart = DateTime.Max(adjustment.Period.From, period.From);
            var adjEnd = DateTime.Min(adjustment.Period.To, period.To);

            if (adjStart >= adjEnd) continue; 

            var adjRange = new DateTimeRange(adjStart, adjEnd);

            if (adjustment.Type == AdjustmentType.Override)
                result = [adjRange];
            else
                result = result.SubtractRanges(adjRange);
        }

        return result.MergeRanges();
    }

    private static IReadOnlyCollection<DateTimeRange> ApplyEmployments(
        IReadOnlyCollection<DateTimeRange> slots,
        IEnumerable<EmploymentSnapshot> employments,
        DateTimeRange period)
    {
        var mergedEmployments = employments
            .Select(e => e.Period)
            .IntersectWith(period)
            .MergeRanges();
        
        if (mergedEmployments.Count == 0)
            return slots;

        var result = new List<DateTimeRange>();
        foreach (var orderedSlot in slots)
        {
            var potentiallyIntersecting = mergedEmployments.Where(x => x.From < orderedSlot.To);

            var current = orderedSlot.From;

            foreach (var employment in potentiallyIntersecting)
            {
                if (employment.From > current)
                    result.Add(new DateTimeRange(current, employment.From));

                current = DateTime.Max(current, employment.To);
            }

            if (current < orderedSlot.To)
                result.Add(new DateTimeRange(current, orderedSlot.To));
        }
        
        return result.MergeRanges();
    }
}
