using CC.PractitionerService.Domain.Availability;
using CC.PractitionerService.Domain.Availability.Services;
using CC.PractitionerService.Domain.WorkSchedules;
using CC.Shared.Domain.TimeRanges;

namespace CC.PractitionerTests.Availability;

public class PractitionerAvailabilityCalculatorTests
{
    private readonly PractitionerAvailabilityCalculator _sut = new();

    [Fact]
    public void Availability_includes_scheduled_work_hours_clipped_to_requested_period()
    {
        var period = new DateTimeRange(new DateTime(2026, 4, 13, 10, 0, 0), new DateTime(2026, 4, 13, 12, 0, 0));
        var schedule = CreateWorkSchedule(DayOfWeek.Monday, new TimeOnly(8, 0), new TimeOnly(18, 0));

        var result = _sut.Calculate([schedule], [], [], period);

        var r = Assert.Single(result);
        Assert.Equal(new DateTime(2026, 4, 13, 10, 0, 0), r.From);
        Assert.Equal(new DateTime(2026, 4, 13, 12, 0, 0), r.To);
    }

    [Fact]
    public void Availability_excludes_hours_when_schedule_is_outside_validity_period()
    {
        var period = new DateTimeRange(new DateTime(2026, 5, 1), new DateTime(2026, 5, 8));
        var schedule = CreateWorkSchedule(DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(17, 0), validTo: new DateOnly(2026, 4, 30));

        var result = _sut.Calculate([schedule], [], [], period);

        Assert.Empty(result);
    }

    [Fact]
    public void Availability_merges_overlapping_schedules_into_single_continuous_block()
    {
        var period = new DateTimeRange(new DateTime(2026, 4, 13), new DateTime(2026, 4, 14));
        var morningSchedule = CreateWorkSchedule(DayOfWeek.Monday, new TimeOnly(8, 0), new TimeOnly(14, 0));
        var eveningSchedule = CreateWorkSchedule(DayOfWeek.Monday, new TimeOnly(12, 0), new TimeOnly(18, 0));

        var result = _sut.Calculate([morningSchedule, eveningSchedule], [], [], period);

        var r = Assert.Single(result);
        Assert.Equal(new DateTime(2026, 4, 13, 8, 0, 0), r.From);
        Assert.Equal(new DateTime(2026, 4, 13, 18, 0, 0), r.To);
    }

    [Fact]
    public void Availability_removes_unavailable_time_blocks_from_scheduled_hours()
    {
        var period = new DateTimeRange(new DateTime(2026, 4, 13, 9, 0, 0), new DateTime(2026, 4, 13, 17, 0, 0));
        var schedule = CreateWorkSchedule(DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(17, 0));
        var adjustment = new Adjustment(
            Guid.NewGuid(), schedule.Id, AdjustmentType.Unavailable,
            new DateTimeRange(new DateTime(2026, 4, 13, 12, 0, 0), new DateTime(2026, 4, 13, 13, 0, 0)));

        var result = _sut.Calculate([schedule], [adjustment], [], period).ToArray();

        Assert.Equal(2, result.Length);
        Assert.Equal(new DateTime(2026, 4, 13, 9, 0, 0), result[0].From);
        Assert.Equal(new DateTime(2026, 4, 13, 12, 0, 0), result[0].To);
        Assert.Equal(new DateTime(2026, 4, 13, 13, 0, 0), result[1].From);
        Assert.Equal(new DateTime(2026, 4, 13, 17, 0, 0), result[1].To);
    }

    [Fact]
    public void Availability_replaces_original_hours_with_override_adjustment()
    {
        var period = new DateTimeRange(new DateTime(2026, 4, 13, 9, 0, 0), new DateTime(2026, 4, 13, 17, 0, 0));
        var schedule = CreateWorkSchedule(DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(17, 0));
        var adjustment = new Adjustment(
            Guid.NewGuid(), schedule.Id, AdjustmentType.Override,
            new DateTimeRange(new DateTime(2026, 4, 13, 14, 0, 0), new DateTime(2026, 4, 13, 15, 0, 0)));

        var result = _sut.Calculate([schedule], [adjustment], [], period);

        var r = Assert.Single(result);
        Assert.Equal(new DateTime(2026, 4, 13, 14, 0, 0), r.From);
        Assert.Equal(new DateTime(2026, 4, 13, 15, 0, 0), r.To);
    }

    [Fact]
    public void Availability_excludes_time_covered_by_employment_restrictions()
    {
        var period = new DateTimeRange(new DateTime(2026, 4, 13, 9, 0, 0), new DateTime(2026, 4, 13, 17, 0, 0));
        var schedule = CreateWorkSchedule(DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(17, 0));
        var employment = new EmploymentSnapshot(
            schedule.PractitionerId, string.Empty,
            new DateTimeRange(new DateTime(2026, 4, 13, 12, 0, 0), new DateTime(2026, 4, 13, 14, 0, 0)),
            DateTime.MinValue);

        var result = _sut.Calculate([schedule], [], [employment], period).ToArray();

        Assert.Equal(2, result.Length);
        Assert.Equal(new DateTime(2026, 4, 13, 9, 0, 0), result[0].From);
        Assert.Equal(new DateTime(2026, 4, 13, 12, 0, 0), result[0].To);
        Assert.Equal(new DateTime(2026, 4, 13, 14, 0, 0), result[1].From);
        Assert.Equal(new DateTime(2026, 4, 13, 17, 0, 0), result[1].To);
    }

    [Fact]
    public void Availability_returns_empty_when_no_schedules_are_provided()
    {
        var period = new DateTimeRange(new DateTime(2026, 4, 13), new DateTime(2026, 4, 20));

        var result = _sut.Calculate([], [], [], period);

        Assert.Empty(result);
    }

    [Fact]
    public void Availability_ignores_adjustments_outside_the_requested_period()
    {
        var period = new DateTimeRange(new DateTime(2026, 4, 13, 9, 0, 0), new DateTime(2026, 4, 13, 17, 0, 0));
        var schedule = CreateWorkSchedule(DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(17, 0));
        var adjustment = new Adjustment(
            Guid.NewGuid(), schedule.Id, AdjustmentType.Unavailable,
            new DateTimeRange(new DateTime(2026, 4, 20, 9, 0, 0), new DateTime(2026, 4, 20, 10, 0, 0)));

        var result = _sut.Calculate([schedule], [adjustment], [], period);

        var r = Assert.Single(result);
        Assert.Equal(new DateTime(2026, 4, 13, 9, 0, 0), r.From);
        Assert.Equal(new DateTime(2026, 4, 13, 17, 0, 0), r.To);
    }

    private static WorkSchedule CreateWorkSchedule(
        DayOfWeek dayOfWeek, TimeOnly start, TimeOnly end, DateOnly? validTo = null)
    {
        return new WorkSchedule(
            Guid.NewGuid(),
            Guid.NewGuid(),
             validTo is not null ? new(new DateOnly(2026, 1, 1), validTo.Value) : new(new DateOnly(2026, 1, 1)),
            TimeZoneInfo.Utc,
            [new WeeklyRecurrence(dayOfWeek, new TimeRange(start, end))]);
    }
}