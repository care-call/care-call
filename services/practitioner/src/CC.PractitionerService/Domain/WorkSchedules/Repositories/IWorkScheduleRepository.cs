namespace CC.PractitionerService.Domain.WorkSchedules.Repositories;

public interface IWorkScheduleRepository
{
    Task<WorkSchedule?> GetAsync(Guid workScheduleId);
    Task AddAsync(WorkSchedule workSchedule);
    Task<WorkSchedule?> GetActiveForPractitionerAsync(Guid practitionerId);
}