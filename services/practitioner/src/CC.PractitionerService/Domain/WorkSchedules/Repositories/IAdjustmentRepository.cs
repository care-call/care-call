namespace CC.PractitionerService.Domain.WorkSchedules.Repositories;

public interface IAdjustmentRepository
{
    public Task AddAsync(ICollection<Adjustment> adjustments);

    public Task<IReadOnlyList<Adjustment>> ListByScheduleIdAsync(
        Guid scheduleId);
}