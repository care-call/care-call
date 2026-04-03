using System.ComponentModel.DataAnnotations;
using CC.AppointmentService.Domain.Feedbacks.Rules;

namespace CC.AppointmentService.Api.Contracts.Feedback;

public record CreateFeedbackRequest(
    Guid AppointmentId,
    Guid ClientId,
    byte ComfortScore, 
    byte ProfessionalismScore, 
    byte EmpathyScore, 
    IReadOnlyCollection<Guid> Tags) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (ComfortScore is < 1 or > 5)
        {
            yield return new ValidationResult(
                $"Комфортность должна быть оценена от 1 до 5",
                [nameof(ComfortScore)]);
        }

        if (ProfessionalismScore is < 1 or > 5)
        {
            yield return new ValidationResult(
                $"Профессионализм должен быть оценен от 1 до 5",
                [nameof(ProfessionalismScore)]);
        }

        if (ProfessionalismScore is < 1 or > 5)
        {
            yield return new ValidationResult(
                $"Эмпатия должна быть оценена от 1 до 5",
                [nameof(EmpathyScore)]);
        }
    }
}
