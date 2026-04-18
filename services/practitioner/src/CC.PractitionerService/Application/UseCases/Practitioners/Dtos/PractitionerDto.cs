using CC.Shared.Domain.TimeRanges;

namespace CC.PractitionerService.Application.UseCases.Practitioners.Dtos;

public sealed record PractitionerDto
{
    public Guid Id { get; init; }
    public string? PhotoUrl { get; init; }
    public required string Bio { get; init; }
    public required string FullName { get; init; }
    public int Status { get; init; }
    public IReadOnlyList<int> ProblemAreas { get; init; } = [];
    public IReadOnlyList<int> AgeGroups { get; init; } = [];
    public IReadOnlyList<int> Languages { get; init; } = [];
    public float Rating { get; init; }
    public IReadOnlyList<DateTimeRange> Availabilities { get; init; } = [];
}