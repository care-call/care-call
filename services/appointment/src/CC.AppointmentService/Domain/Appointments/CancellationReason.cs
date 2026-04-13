using Vogen;

namespace CC.AppointmentService.Domain.Appointments;

/// <summary>
/// Причина отмены записи.
/// </summary>
[ValueObject<string>]
public readonly partial struct CancellationReason
{
    private static Validation Validate(string value) =>
        string.IsNullOrWhiteSpace(value)
            ? Validation.Invalid("Причина отмены не может быть пустой")
            : value.Length > 2000
                ? Validation.Invalid("Причина отмены не может превышать 2000 символов")
                : Validation.Ok;
}
