using CC.Common.Models;
using CC.PractitionerService.Application.Dependencies;
using CC.PractitionerService.Application.UseCases.Practitioners.Dtos;
using FluentResults;

namespace CC.PractitionerService.Application.UseCases.Practitioners;

public sealed record AvailablePractitionerFilter
{
    public DateOnly PeriodFrom { get; init; }
    public DateOnly PeriodTo { get; init; }
    public IReadOnlyCollection<int>? AgeGroupIds { get; init; }
    public IReadOnlyCollection<int>? ProblemAreas { get; init; }
    public IReadOnlyCollection<int>? PractitionerLanguages { get; init; }
    public string? PractitionerFullName { get; init; }
    public PageInfo PageInfo { get; init; } = new(1, 20);
}

public sealed class GetAvailablePractitionersUseCase(IAvailablePractitionersQuery availablePractitionersQuery)
{
    public async ValueTask<Result<PractitionerDto[]>> Handle(AvailablePractitionerFilter filter, CancellationToken ct)
        => Result.Ok(await availablePractitionersQuery.FindAvailableAsync(filter, ct));
}
