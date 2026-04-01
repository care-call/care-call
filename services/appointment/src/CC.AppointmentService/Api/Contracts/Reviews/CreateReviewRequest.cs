namespace CC.AppointmentService.Api.Contracts.Reviews;

public record CreateReviewRequest(
    Guid AppointmentId,
    Guid ClientId,
    byte ComfortScore, 
    byte ProfessionalismScore, 
    byte EmpathyScore, 
    IReadOnlyCollection<Guid> Tags);