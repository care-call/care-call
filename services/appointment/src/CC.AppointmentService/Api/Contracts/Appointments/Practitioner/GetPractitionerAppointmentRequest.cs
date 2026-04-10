using System.ComponentModel.DataAnnotations;
using CC.AppointmentService.Application.UseCases.Appointments.Getting;

namespace CC.AppointmentService.Api.Contracts.Appointments.Practitioner;

public sealed record GetPractitionerAppointmentRequest : IValidatableObject
{
    public Guid PractitionerId { get; init; }
    public PractitionerAppointmentsDateFilter DateFilter { get; init; } = PractitionerAppointmentsDateFilter.All;
    public PractitionerAppointmentsStateFilter StateFilter { get; init; } = PractitionerAppointmentsStateFilter.Upcoming;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!Enum.IsDefined(DateFilter))
        {
            yield return new ValidationResult(
                "Несуществует такой временной фильтр.",
                [nameof(DateFilter)]);
        }

        if (!Enum.IsDefined(StateFilter))
        {
            yield return new ValidationResult(
                "Несуществует такой фильтр состояния.",
                [nameof(StateFilter)]);
        }
    }
}