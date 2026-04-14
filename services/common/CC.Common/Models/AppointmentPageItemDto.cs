namespace CC.Common.Models;

public sealed record AppointmentPageItemDto
{
    public Guid Id { get; set; }
    public required string PractitionerFullName { get; set; }
    public required DateTime DateOfEvent { get; set; }
    public string PsychologicalProblems { get; set; } = string.Empty;
}