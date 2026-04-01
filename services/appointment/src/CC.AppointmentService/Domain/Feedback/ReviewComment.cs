using Vogen;

namespace CC.AppointmentService.Domain.Feedback;

[ValueObject<string>]
public readonly partial struct ReviewComment
{
    private static Validation Validate(string value) =>
        value.Length > 1000 ? 
            Validation.Invalid("Комментарии не может быть больше 1000 символов") : 
            Validation.Ok;
}