using CC.AppointmentService.Domain.Feedbacks;
using CC.Common.Errors;
using FluentResults;

namespace CC.AppointmentService.Domain.Errors;

public static class FeedbackError
{
    public static Error TooLate =>
        new Error($"Вы можете оставить отзыв только в течение {Feedback.CreationWindowDays} суток")
            .WithErrorCode("F101");

    public static Error AppointmentNotCompleted =>
        new Error("Клиент не может оставить отзыв на незавершенную запись")
            .WithErrorCode("F102");

    public static Error AlreadyExists =>
        new Error("Клиент не может оставить несколько отзывов на одну запись")
            .WithErrorCode("F103");
}
