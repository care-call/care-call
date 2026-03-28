using CC.AppointmentService.Domain.Appointments;

namespace CC.AppointmentService.Domain.Reviews.Rules;

public static class SessionReviewRules
{
    public const int MaxDayReviewAfterSession = 2;

    public static bool IsStatusCreatable(Appointment appointment) =>
        appointment.Status != AppointmentStatus.Completed || appointment.EndedAt is null;

    public static bool IsWithinAllowedReviewing(Appointment appointment) =>
        appointment.EndedAt == null || appointment.EndedAt.Value.AddDays(MaxDayReviewAfterSession) < DateTime.Now;
    
}