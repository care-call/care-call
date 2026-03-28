namespace CC.AppointmentService.Api.Contracts;

public sealed record CreateSessionReviewRequest
{
    public required Guid AppointmentId { get; init; }
    public required Guid UserId { get; init; }
    public required int EmpathyRating { get; init; }
    public required int ComfortRating { get; init; }
    public required int ProfessionalismRating { get; init; }
    public required string? ReviewComment { get; init; }
}
