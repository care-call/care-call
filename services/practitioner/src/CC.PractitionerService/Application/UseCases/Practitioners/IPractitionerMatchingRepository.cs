using CC.PractitionerService.Application.UseCases.Practitioners.Dtos;

namespace CC.PractitionerService.Application.UseCases.Practitioners;

public interface IPractitionerMatchingRepository
{
    Task<PractitionerDto[]> FindAvailableForBookingAsync(
        MatchPractitionersFilter filter,
        CancellationToken ct);
}
