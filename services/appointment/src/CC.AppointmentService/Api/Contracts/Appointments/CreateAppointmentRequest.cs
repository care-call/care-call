using System.ComponentModel.DataAnnotations;
using CC.AppointmentService.Domain.Appointments;

namespace CC.AppointmentService.Api.Contracts.Appointments;

public sealed record CreateAppointmentRequest : IValidatableObject
{
    public required Guid ClientId { get; init; }
    public required Guid PractitionerId { get; init; }
    public DateTime StartedAt { get; init; }
    public DateTime EndedAt { get; init; }
    public required ClientSnapshot ClientSnapshot { get; init; }
    public required PractitionerSnapshot PractitionerSnapshot { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (StartedAt >= EndedAt)
            yield return new ValidationResult("Некорректно указаны даты записи.", [nameof(StartedAt), nameof(EndedAt)]);
    }
}