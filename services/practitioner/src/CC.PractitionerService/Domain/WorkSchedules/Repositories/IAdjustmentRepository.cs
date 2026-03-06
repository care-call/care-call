using CC.Shared.Domain.TimeRanges;

namespace CC.PractitionerService.Domain.WorkSchedules.Repositories;

public interface IAdjustmentRepository
{
    public Task AddAsync(ICollection<Adjustment> adjustments);
    public Task<List<Adjustment>> GetWeeklyAsync(Guid scheduleId, Week week);
    public void Remove(ICollection<Adjustment> adjustments);
}