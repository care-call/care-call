using Vogen;

namespace CC.AppointmentService.Domain.Feedbacks;

[ValueObject<string>]
public readonly partial struct FeedbackComment
{
    private static Validation Validate(string value) =>
        value.Length > 1000 ? 
            Validation.Invalid("Комментарии не может составлять больше 1000 символов") : 
            Validation.Ok;
}