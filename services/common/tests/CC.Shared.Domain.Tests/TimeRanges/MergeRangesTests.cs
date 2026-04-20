using CC.Shared.Domain.Tests.TimeRanges.Extensions;
using CC.Shared.Domain.Tests.TimeRanges.TestData;
using CC.Shared.Domain.TimeRanges;
using CC.Shared.Domain.TimeRanges.Extensions;

namespace CC.Shared.Domain.Tests.TimeRanges;

public sealed class MergeRangesTests
{
    [Fact]
    public void Overlapping_time_ranges_are_merged_into_one_continuous_range()
    {
        DateTimeRange[] ranges = [
            DateTimeRange.Range("08:00", "14:00"),
            DateTimeRange.Range("13:00", "17:00")
        ];

        var result = ranges.MergeRanges();

        var r = Assert.Single(result);
        Assert.Equal(DateTimeRange.Range("08:00", "17:00"), r);
    }

    [Fact]
    public void Adjacent_time_ranges_are_merged_without_breaking()
    {
        DateTimeRange[] ranges = [
            DateTimeRange.Range("08:00", "14:00"),
            DateTimeRange.Range("14:00", "17:00")
        ];

        var result = ranges.MergeRanges();

        var r = Assert.Single(result);
        Assert.Equal(DateTimeRange.Range("08:00", "17:00"), r);
    }

    [Fact]
    public void Nested_time_ranges_collapse_into_single_encompassing_block()
    {
        DateTimeRange[] ranges = [
            DateTimeRange.Range("08:00", "14:00"),
            DateTimeRange.Range("09:00", "13:00")
        ];

        var result = ranges.MergeRanges();

        var r = Assert.Single(result);
        Assert.Equal(DateTimeRange.Range("08:00", "14:00"), r);
    }

    [Theory]
    [ClassData(typeof(DisjointTimeRangesTestData))]
    public void Disjoint_time_ranges_remain_separate(DateTimeRange[] ranges, DateTimeRange[] expected)
    {
        var result = ranges.MergeRanges().ToArray();

        Assert.Equal(expected.Length, result.Length);
        Assert.Equal(expected[0], result[0]);
        Assert.Equal(expected[1], result[1]);
    }

    [Fact]
    public void Unsorted_ranges_are_merged_correctly()
    {
        DateTimeRange[] ranges = [
            DateTimeRange.Range("15:00", "18:00"),
            DateTimeRange.Range("08:00", "14:30"),
            DateTimeRange.Range("14:00", "15:00")
        ];

        var result = ranges.MergeRanges();

        var r = Assert.Single(result);
        Assert.Equal(DateTimeRange.Range("08:00", "18:00"), r);
    }

    [Fact]
    public void Empty_collection_yields_empty_result()
    {
        DateTimeRange[] ranges = [];

        var result = ranges.MergeRanges();

        Assert.Empty(result);
    }
}
