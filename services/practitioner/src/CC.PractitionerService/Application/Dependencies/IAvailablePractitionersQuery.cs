using CC.PractitionerService.Application.UseCases.Practitioners;
using CC.PractitionerService.Application.UseCases.Practitioners.Dtos;

namespace CC.PractitionerService.Application.Dependencies;

public interface IAvailablePractitionersQuery
{
    Task<PractitionerDto[]> FindAvailableAsync(
        AvailablePractitionerFilter filter,
        CancellationToken ct);
}
