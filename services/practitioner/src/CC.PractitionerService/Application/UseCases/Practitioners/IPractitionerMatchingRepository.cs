using CC.PractitionerService.Application.UseCases.WorkSchedules.Dtos;

namespace CC.PractitionerService.Application.UseCases.Practitioners;

public interface IPractitionerMatchingRepository
{
    Task<PractitionerDto[]> FindAvailableForBookingAsync(
        FindPractitionersFilter filter,
        CancellationToken ct);
}
