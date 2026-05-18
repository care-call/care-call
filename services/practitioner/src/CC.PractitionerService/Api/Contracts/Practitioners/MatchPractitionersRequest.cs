namespace CC.PractitionerService.Api.Contracts.Practitioners;

/// <summary>
/// Запрос на подбор практиканта для записи.
/// </summary>
public sealed record MatchPractitionersRequest
{
    public DateOnly PeriodFrom { get; init; }
    public DateOnly PeriodTo { get; init; }
    public IReadOnlyCollection<int>? AgeGroupIds { get; init; }
    public IReadOnlyCollection<int>? ProblemAreas { get; init; }
    public IReadOnlyCollection<int>? PractitionerLanguages { get; init; }
    public string? PractitionerFullName { get; init; }
}