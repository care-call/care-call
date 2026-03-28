using CC.AppointmentService.Domain.Appointments;
using FluentResults;

namespace CC.AppointmentService.Domain.Reviews.Rules;

public static class SessionReviewRules
{
    public const int MaxDayReviewAfterSession = 2;
    public static Result CanCreate(Appointment reviewAppointment)
    {
        if (reviewAppointment.Status != AppointmentStatus.Completed || reviewAppointment.EndedAt is null)
            return Result.Fail("Отзыв можно оставить только по завершенным сессиям");

        if (reviewAppointment.EndedAt.Value.AddDays(MaxDayReviewAfterSession) < DateTime.Now)
            return Result.Fail("Отзыв можно оставить только в течение 2 суток после завершения сессии.");
        
        return Result.Ok(); 
    }
}