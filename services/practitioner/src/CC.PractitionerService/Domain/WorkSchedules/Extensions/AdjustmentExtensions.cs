namespace CC.PractitionerService.Domain.WorkSchedules.Extensions;

public static class AdjustmentExtensions
{
    extension(IEnumerable<Adjustment> adjustments)
    {
        public IEnumerable<Adjustment> DetermineConflicts()
        {
            var ordered = adjustments.OrderBy(a => a.Period.From).ThenBy(a => a.Period.To).ToList();
            return ordered.Where(adjustment =>
            {
                var currentIndex = ordered.IndexOf(adjustment);
                
                var nextAdjustment = ordered.ElementAtOrDefault(currentIndex + 1);
                var previousAdjustment = ordered.ElementAtOrDefault(currentIndex - 1);
                
                return nextAdjustment is not null && adjustment.Intersects(nextAdjustment) ||
                       previousAdjustment is not null && adjustment.Intersects(previousAdjustment);
            });
        }
        
        public IEnumerable<Adjustment> ClearDuplicates()
        {
            return adjustments.DistinctBy(a => new { a.Period.From, a.Period.To, a.Type });
        }
    }
}