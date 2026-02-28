using CC.PractitionerService.Domain.WorkSchedules;
using CC.PractitionerService.Domain.WorkSchedules.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CC.PractitionerService.Infrastructure.Persistence.Adjustments;

public class AdjustmentRepository(DatabaseContext db) : IAdjustmentRepository
{
    public async Task AddAsync(ICollection<Adjustment> adjustments) =>
        await db.AddRangeAsync(adjustments);

    public async Task<IReadOnlyList<Adjustment>> ListByScheduleIdAsync(Guid scheduleId) =>
        await db.Adjustments.Where(a => a.WorkScheduleId == scheduleId).ToListAsync();
}