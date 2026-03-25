using CC.PractitionerService.Application.UseCases.WorkSchedules.Dtos;
using FluentResults;
using Mediator;

namespace CC.PractitionerService.Application.UseCases.Practitioners;

public sealed record FindPractitionersFilter : IRequest<Result<PractitionerDto[]>>
{
    public DateOnly? TargetDate { get; init; }
    public IReadOnlyCollection<int>? AgeGroupIds { get; init; }
    public IReadOnlyCollection<int>? ProblemAreas { get; init; }
    public IReadOnlyCollection<int>? PractitionerLanguages { get; init; }
    public string? PractitionerFullName { get; init; }
    public int PageSize { get; init; }
    public int PageNumber { get; init; }
}

public sealed class FindPractitionersUseCase(IPractitionerMatchingRepository _practitionerMatchingRepository) : IRequestHandler<FindPractitionersFilter, Result<PractitionerDto[]>>
{
    public async ValueTask<Result<PractitionerDto[]>> Handle(FindPractitionersFilter filter, CancellationToken ct)
    {
        var practitioners = await _practitionerMatchingRepository.FindAvailableForBookingAsync(filter, ct);

        if (filter.TargetDate is not null)
        {
            //TODO: Реализовать фильтрацию по занятности
        }

        return Result.Ok(practitioners);
    }
}
