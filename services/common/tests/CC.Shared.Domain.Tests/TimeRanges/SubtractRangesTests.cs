using CC.Shared.Domain.Tests.TimeRanges.Extensions;
using CC.Shared.Domain.TimeRanges;
using CC.Shared.Domain.TimeRanges.Extensions;

namespace CC.Shared.Domain.Tests.TimeRanges;

public sealed class SubtractRangesTests
{
    [Fact]
    public void Time_range_with_no_overlap_remains_unchanged()
    {
        var ranges = new[] { DateTimeRange.Range("08:00", "12:00") };
        var subtract = DateTimeRange.Range("14:00", "16:00");

        var result = ranges.SubtractRanges(subtract);

        var r = Assert.Single(result);
        Assert.Equal(DateTimeRange.Range("08:00", "12:00"), r);
    }

    [Fact]
    public void Range_fully_covered_by_subtract_is_removed()
    {
        var ranges = new[] { DateTimeRange.Range("08:00", "12:00") };
        var subtract = DateTimeRange.Range("07:00", "15:00");

        var result = ranges.SubtractRanges(subtract);

        Assert.Empty(result);
    }

    [Fact]
    public void Subtract_at_start_trims_beginning_of_range()
    {
        var ranges = new[] { DateTimeRange.Range("08:00", "16:00") };
        var subtract = DateTimeRange.Range("06:00", "10:00");

        var result = ranges.SubtractRanges(subtract);

        var r = Assert.Single(result);
        Assert.Equal(DateTimeRange.Range("10:00", "16:00"), r);
    }

    [Fact]
    public void Subtract_at_end_trims_tail_of_range()
    {
        var ranges = new[] { DateTimeRange.Range("08:00", "16:00") };
        var subtract = DateTimeRange.Range("12:00", "18:00");

        var result = ranges.SubtractRanges(subtract);

        var r = Assert.Single(result);
        Assert.Equal(DateTimeRange.Range("08:00", "12:00"), r);
    }

    [Fact]
    public void Subtract_in_middle_splits_range_into_two_parts()
    {
        var ranges = new[] { DateTimeRange.Range("08:00", "18:00") };
        var subtract = DateTimeRange.Range("12:00", "14:00");

        var result = ranges.SubtractRanges(subtract).ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal(DateTimeRange.Range("08:00", "12:00"), result[0]);
        Assert.Equal(DateTimeRange.Range("14:00", "18:00"), result[1]);
    }
}
