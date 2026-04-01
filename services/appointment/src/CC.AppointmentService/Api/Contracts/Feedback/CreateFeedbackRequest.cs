namespace CC.AppointmentService.Api.Contracts.Feedback;

public record CreateFeedbackRequest(
    Guid AppointmentId,
    Guid ClientId,
    byte ComfortScore, 
    byte ProfessionalismScore, 
    byte EmpathyScore, 
    IReadOnlyCollection<Guid> Tags);