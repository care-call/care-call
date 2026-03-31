namespace CC.AppointmentService.Api.Contracts.Review;

public record CreateReviewRequest(
    Guid AppointmentId,
    byte ComfortScore, 
    byte ProfessionalismScore, 
    byte EmpathyScore, 
    List<Guid> Tags);