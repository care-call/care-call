using CC.PractitionerService.Domain.WorkSchedules;
using CC.PractitionerService.Domain.WorkSchedules.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CC.PractitionerService.Infrastructure.Persistence.WorkSchedules;

internal sealed class WorkScheduleRepository(DatabaseContext db) : IWorkScheduleRepository
{
    public Task<WorkSchedule?> GetAsync(Guid workScheduleId) => db.WorkSchedules.FirstOrDefaultAsync(w => w.Id == workScheduleId);

    public async Task AddAsync(WorkSchedule workSchedule) => await db.WorkSchedules.AddAsync(workSchedule);

    public Task<WorkSchedule?> GetActiveForPractitionerAsync(Guid practitionerId) =>
        db.WorkSchedules.FirstOrDefaultAsync(w =>
            w.PractitionerId == practitionerId && w.Status == WorkScheduleStatus.Active);
}