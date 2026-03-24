using CC.Shared.Domain.TimeRanges;

namespace CC.PractitionerService.Application.UseCases.Practitioners;

public interface ISlotsReadRepository
{
    Task<IReadOnlyDictionary<Guid, IReadOnlyList<DateTimeRange>>> GetFreeSlotsAsync(
        IReadOnlyCollection<Guid> practitionerIds,
        DateOnly targetDate,
        CancellationToken ct);
}