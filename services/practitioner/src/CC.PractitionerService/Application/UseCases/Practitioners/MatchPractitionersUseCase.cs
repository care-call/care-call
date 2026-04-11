using CC.PractitionerService.Application.Dependencies;
using CC.PractitionerService.Application.UseCases.Practitioners.Dtos;
using FluentResults;

namespace CC.PractitionerService.Application.UseCases.Practitioners;

public sealed record AvailablePractitionerFilter
{
    public DateOnly? TargetDate { get; init; }
    public IReadOnlyCollection<int>? AgeGroupIds { get; init; }
    public IReadOnlyCollection<int>? ProblemAreas { get; init; }
    public IReadOnlyCollection<int>? PractitionerLanguages { get; init; }
    public string? PractitionerFullName { get; init; }
    public int PageSize { get; init; } = 20;
    public int PageNumber { get; init; }
}

public sealed class MatchPractitionersUseCase(IAvailablePractitionersQuery availablePractitionersQuery)
{
    public async ValueTask<Result<PractitionerDto[]>> Handle(AvailablePractitionerFilter filter, CancellationToken ct)
    {
        var practitioners = await availablePractitionersQuery.FindAvailableAsync(filter, ct);

        if (filter.TargetDate is not null)
        {
            //TODO: Реализовать фильтрацию по занятности
        }

        return Result.Ok(practitioners);
    }
}
