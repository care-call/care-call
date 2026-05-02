using CC.PractitionerService.Domain.WorkSchedules;
using CC.PractitionerService.Domain.WorkSchedules.Repositories;
using CC.Shared.Domain.TimeRanges;
using Microsoft.EntityFrameworkCore;

namespace CC.PractitionerService.Infrastructure.Persistence.Adjustments;

public class AdjustmentRepository(DatabaseContext db) : IAdjustmentRepository
{
    public Task AddAsync(ICollection<Adjustment> adjustments) 
        => db.AddRangeAsync(adjustments);

    public Task<List<Adjustment>> GetWeeklyAsync(Guid scheduleId, Week week) 
        => db.Adjustments.Where(a => a.WorkScheduleId == scheduleId &&
                                        a.Period.From >= week.StartedAt
                                        && a.Period.To <= week.EndedAt).ToListAsync();

    public void Remove(ICollection<Adjustment> adjustments) =>
        db.Adjustments.RemoveRange(adjustments);
}