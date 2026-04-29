namespace CC.Shared.Domain.TimeRanges.Extensions;

public static class DateTimeRangeExtensions
{
    extension(DateTimeRange)
    {
        public static DateTimeRange? GetIntersection(DateTimeRange a, DateTimeRange b)
        {
            var start = DateTime.Max(a.From, b.From);
            var end = DateTime.Min(a.To, b.To);

            return start >= end ? null : new DateTimeRange(start, end);
        }
    }

    extension(IEnumerable<DateTimeRange> ranges)
    {
        public IReadOnlyCollection<DateTimeRange> MergeRanges()
        {
            var sorted = ranges.OrderBy(r => r.From).ToArray();
            
            if (sorted.Length == 0)
                return [];

            var merged = new List<DateTimeRange>();

            var current = sorted[0];

            for (var i = 1; i < sorted.Length; i++)
            {
                if (sorted[i].From <= current.To)
                {
                    current = new DateTimeRange(current.From, DateTime.Max(current.To, sorted[i].To));
                }
                else
                {
                    merged.Add(current);
                    current = sorted[i];
                }
            }
            merged.Add(current);
            return merged;
        }

        public IEnumerable<DateTimeRange> SubtractRanges(DateTimeRange subtract)
        {
            foreach (var range in ranges)
            {
                if (range.To <= subtract.From || range.From >= subtract.To)
                {
                    yield return range;
                    continue;
                }

                if (range.From >= subtract.From && range.To <= subtract.To)
                    continue;

                if (range.From < subtract.From)
                    yield return new DateTimeRange(range.From, subtract.From);

                if (range.To > subtract.To)
                    yield return new DateTimeRange(subtract.To, range.To);
            }
        }

        public IEnumerable<DateTimeRange> IntersectWith(DateTimeRange intersect)
        {
            foreach (var r in ranges)
            {
                var from = DateTime.Max(r.From, intersect.From);
                var to = DateTime.Min(r.To, intersect.To);

                if (from < to)
                    yield return new DateTimeRange(from, to);
            }
        }
    }
}