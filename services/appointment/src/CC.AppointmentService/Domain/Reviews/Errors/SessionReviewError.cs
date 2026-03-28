using CC.AppointmentService.Domain.Reviews.Rules;
using CC.Common.Errors;
using FluentResults;

namespace CC.AppointmentService.Domain.Reviews.Errors;

public static class SessionReviewError
{
    public static Error NotCompletedSessionError =>
        new Error("Отзыв можно оставить только по завершенным сессиям")
            .WithErrorCode("SR101");

    public static Error TooLateReview =>
        new Error($"Отзыв можно оставить только в течение {SessionReviewRules.MaxDayReviewAfterSession} суток после завершения сессии")
            .WithErrorCode("SR102");
    
    public static Error ReviewAlreadyExists =>
        new Error("Отзыв уже существует для этой сессии")
            .WithErrorCode("SR103");
    
}