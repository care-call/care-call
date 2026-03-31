using Vogen;

namespace CC.AppointmentService.Domain.Reviews;

[ValueObject<byte>]
public readonly partial struct ComfortScore
{
    private static Validation Validate(byte value) =>
        value is < 1 or > 5 ? 
            Validation.Invalid("оценка не может быть меньше 0 или же больше 5") :
            Validation.Ok;
}