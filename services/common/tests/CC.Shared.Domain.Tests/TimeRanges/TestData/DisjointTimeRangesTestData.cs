using System.Collections;
using CC.Shared.Domain.Tests.TimeRanges.Extensions;
using CC.Shared.Domain.TimeRanges;

namespace CC.Shared.Domain.Tests.TimeRanges.TestData;

public class DisjointTimeRangesTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new DateTimeRange[] {
                DateTimeRange.Range("08:00:00", "14:00:00"),
                DateTimeRange.Range("15:00:00", "17:00:00")
            },
            new DateTimeRange[]
            {
                DateTimeRange.Range("08:00:00", "14:00:00"),
                DateTimeRange.Range("15:00:00", "17:00:00")
            }
        };

        yield return new object[]
        {
            new DateTimeRange[]
            {
                DateTimeRange.Range("08:00:00", "14:00:00"),
                DateTimeRange.Range("14:01:00", "17:00:00")
            },
            new DateTimeRange[]
            {
                DateTimeRange.Range("08:00:00", "14:00:00"),
                DateTimeRange.Range("14:01:00", "17:00:00")
            },
        };

        yield return new object[]
        {
            new DateTimeRange[]
            {
                DateTimeRange.Range("08:00:00", "14:00:00"),
                DateTimeRange.Range("14:00:01", "17:00:00")
            },
            new DateTimeRange[]
            {
                DateTimeRange.Range("08:00:00", "14:00:00"),
                DateTimeRange.Range("14:00:01", "17:00:00")
            },
        };

        yield return new object[]
        {
            new DateTimeRange[]
            {
                DateTimeRange.Range("08:00:00", "14:00:00"),
                DateTimeRange.Range("14:00:00.00001", "17:00:00")
            },
            new DateTimeRange[]
            {
                DateTimeRange.Range("08:00:00", "14:00:00"),
                DateTimeRange.Range("14:00:00.00001", "17:00:00")
            },
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}