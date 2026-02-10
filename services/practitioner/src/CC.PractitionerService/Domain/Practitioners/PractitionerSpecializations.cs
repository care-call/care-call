namespace CC.PractitionerService.Domain.Practitioners;

public sealed record PractitionerSpecializations(
    IReadOnlyList<int> ProblemAreas,
    IReadOnlyList<int> AgeGroups,
    IReadOnlyList<int> Languages)
{
    public bool HasProblemArea(int id) => ProblemAreas.Contains(id);
    public bool SupportsLanguage(int id) => Languages.Contains(id);
    public bool SupportsAgeGroup(int id) => AgeGroups.Contains(id);
}