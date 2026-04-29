using CC.PractitionerService.Domain.Availability;
using CC.PractitionerService.Domain.Availability.Services;
using CC.PractitionerService.Domain.WorkSchedules;
using CC.Shared.Domain.TimeRanges;

namespace CC.PractitionerTests.Availability;

public class PractitionerAvailabilityCalculatorTests
{
    private readonly PractitionerAvailabilityCalculator _sut = new();

    private static readonly DateTime TestMondayDay = new(2026, 4, 13);

    [Fact]
    public void Availability_includes_scheduled_work_hours_clipped_to_requested_period()
    {
        var requestedPeriod = new DateTimeRange(TestMondayDay.AddHours(10), TestMondayDay.AddHours(12));
        var schedule = CreateWorkSchedule(TestMondayDay.DayOfWeek, new TimeOnly(8, 0), new TimeOnly(18, 0));

        var result = _sut.Calculate([schedule], [], [], requestedPeriod);

        var availabilityPeriod = Assert.Single(result);
        Assert.Equal(requestedPeriod.From, availabilityPeriod.From);
        Assert.Equal(requestedPeriod.To, availabilityPeriod.To);
    }

    [Fact]
    public void Availability_excludes_hours_when_schedule_is_outside_validity_period()
    {
        var requestedPeriod = new DateTimeRange(new DateTime(2026, 5, 1), new DateTime(2026, 5, 8));
        var schedule = CreateWorkSchedule(TestMondayDay.DayOfWeek, new TimeOnly(9, 0), new TimeOnly(17, 0), validTo: new DateOnly(2026, 4, 30));

        var result = _sut.Calculate([schedule], [], [], requestedPeriod);

        Assert.Empty(result);
    }

    [Fact]
    public void Availability_merges_overlapping_schedules_into_single_continuous_block()
    {
        var requestedPeriod = new DateTimeRange(TestMondayDay, TestMondayDay.AddDays(1));
        var morningSchedule = CreateWorkSchedule(TestMondayDay.DayOfWeek, new TimeOnly(8, 0), new TimeOnly(14, 0));
        var eveningSchedule = CreateWorkSchedule(TestMondayDay.DayOfWeek, new TimeOnly(12, 0), new TimeOnly(18, 0));

        var result = _sut.Calculate([morningSchedule, eveningSchedule], [], [], requestedPeriod);

        var availabilityPeriod = Assert.Single(result);
        Assert.Equal(TestMondayDay.AddHours(8), availabilityPeriod.From);
        Assert.Equal(TestMondayDay.AddHours(18), availabilityPeriod.To);
    }

    [Fact]
    public void Availability_removes_unavailable_time_blocks_from_scheduled_hours()
    {
        var requestedPeriod = new DateTimeRange(TestMondayDay.AddHours(9), TestMondayDay.AddHours(17));
        var schedule = CreateWorkSchedule(TestMondayDay.DayOfWeek, new TimeOnly(9, 0), new TimeOnly(17, 0));

        var adjustment = new Adjustment(
            Guid.Empty,
            schedule.Id,
            AdjustmentType.Unavailable,
            new DateTimeRange(TestMondayDay.AddHours(12), TestMondayDay.AddHours(13)));

        var result = _sut.Calculate([schedule], [adjustment], [], requestedPeriod).ToArray();

        Assert.Equal(2, result.Length);

        var firstAvailabilityPeriod = result[0];
        Assert.Equal(TestMondayDay.AddHours(9), firstAvailabilityPeriod.From);
        Assert.Equal(TestMondayDay.AddHours(12), firstAvailabilityPeriod.To);

        var secondAvailabilityPeriod = result[1];
        Assert.Equal(TestMondayDay.AddHours(13), secondAvailabilityPeriod.From);
        Assert.Equal(TestMondayDay.AddHours(17), secondAvailabilityPeriod.To);
    }

    [Fact]
    public void Availability_replaces_original_hours_with_override_adjustment()
    {
        var requestedPeriod = new DateTimeRange(TestMondayDay.AddHours(9), TestMondayDay.AddHours(17));
        var schedule = CreateWorkSchedule(TestMondayDay.DayOfWeek, new TimeOnly(9, 0), new TimeOnly(17, 0));

        var adjustment = new Adjustment(
            Guid.Empty,
            schedule.Id,
            AdjustmentType.Override,
            new DateTimeRange(TestMondayDay.AddHours(14), TestMondayDay.AddHours(15)));

        var result = _sut.Calculate([schedule], [adjustment], [], requestedPeriod);

        var availabilityPeriod = Assert.Single(result);
        Assert.Equal(TestMondayDay.AddHours(14), availabilityPeriod.From);
        Assert.Equal(TestMondayDay.AddHours(15), availabilityPeriod.To);
    }

    [Fact]
    public void Availability_excludes_time_covered_by_employment_restrictions()
    {
        var requestedPeriod = new DateTimeRange(TestMondayDay.AddHours(9), TestMondayDay.AddHours(17));
        var schedule = CreateWorkSchedule(TestMondayDay.DayOfWeek, new TimeOnly(9, 0), new TimeOnly(17, 0));

        var employment = new EmploymentSnapshot(
            schedule.PractitionerId,
            string.Empty,
            new DateTimeRange(TestMondayDay.AddHours(12), TestMondayDay.AddHours(14)),
            DateTime.MinValue);

        var result = _sut.Calculate([schedule], [], [employment], requestedPeriod).ToArray();

        Assert.Equal(2, result.Length);

        var firstAvailabilityPeriod = result[0];
        Assert.Equal(TestMondayDay.AddHours(9), firstAvailabilityPeriod.From);
        Assert.Equal(TestMondayDay.AddHours(12), firstAvailabilityPeriod.To);

        var secondAvailabilityPeriod = result[1];
        Assert.Equal(TestMondayDay.AddHours(14), secondAvailabilityPeriod.From);
        Assert.Equal(TestMondayDay.AddHours(17), secondAvailabilityPeriod.To);
    }

    [Fact]
    public void Availability_returns_empty_when_no_schedules_are_provided()
    {
        var requestedPeriod = new DateTimeRange(TestMondayDay, TestMondayDay.AddDays(7));

        var result = _sut.Calculate([], [], [], requestedPeriod);

        Assert.Empty(result);
    }

    [Fact]
    public void Availability_ignores_adjustments_outside_the_requested_period()
    {
        var requestedPeriod = new DateTimeRange(TestMondayDay.AddHours(9), TestMondayDay.AddHours(17));
        var schedule = CreateWorkSchedule(TestMondayDay.DayOfWeek, new TimeOnly(9, 0), new TimeOnly(17, 0));

        var nextWeekAdjustment = new Adjustment(
            Guid.Empty,
            schedule.Id,
            AdjustmentType.Unavailable,
            new DateTimeRange(TestMondayDay.AddDays(7).AddHours(9), TestMondayDay.AddDays(7).AddHours(10)));

        var result = _sut.Calculate([schedule], [nextWeekAdjustment], [], requestedPeriod);

        var availabilityPeriod = Assert.Single(result);
        Assert.Equal(TestMondayDay.AddHours(9), availabilityPeriod.From);
        Assert.Equal(TestMondayDay.AddHours(17), availabilityPeriod.To);
    }

    [Fact]
    public void Availability_ignores_out_of_period_employments()
    {
        var requestedPeriod = new DateTimeRange(
            TestMondayDay.AddHours(10),
            TestMondayDay.AddHours(16));

        var schedule = CreateWorkSchedule(
            TestMondayDay.DayOfWeek,
            new TimeOnly(8, 0),
            new TimeOnly(20, 0));
        

        var employment = new EmploymentSnapshot(
            schedule.PractitionerId,
            string.Empty,
            new DateTimeRange(TestMondayDay.AddHours(17), TestMondayDay.AddHours(20)),
            DateTime.MinValue);
        
        var result =  _sut.Calculate(
            [schedule],
            [],
            [employment],
            requestedPeriod);
        
        Assert.Single(result);

        var availability = result.Single();

        Assert.Equal(requestedPeriod.From, availability.From);
        Assert.Equal(requestedPeriod.To, availability.To);
    }

    private static WorkSchedule CreateWorkSchedule(DayOfWeek dayOfWeek, TimeOnly start, TimeOnly end, DateOnly? validTo = null)
        => new(
            Guid.Empty,
            Guid.Empty,
            validTo is not null ? new(new DateOnly(2026, 1, 1), validTo.Value) : new(new DateOnly(2026, 1, 1)),
            TimeZoneInfo.Utc,
            [new WeeklyRecurrence(dayOfWeek, new TimeRange(start, end))]);
}