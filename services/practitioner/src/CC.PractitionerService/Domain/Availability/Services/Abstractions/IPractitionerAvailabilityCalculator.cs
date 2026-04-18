using CC.PractitionerService.Domain.WorkSchedules;
using CC.Shared.Domain.TimeRanges;

namespace CC.PractitionerService.Domain.Availability.Services.Abstractions;

public interface IPractitionerAvailabilityCalculator
{
    IReadOnlyCollection<DateTimeRange> Calculate(
        IReadOnlyCollection<WorkSchedule> workSchedules,
        IReadOnlyCollection<Adjustment> adjustments,
        IReadOnlyCollection<EmploymentSnapshot> employments,
        DateTimeRange period);
}
