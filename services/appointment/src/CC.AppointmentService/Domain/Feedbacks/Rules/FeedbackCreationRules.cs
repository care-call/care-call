using CC.AppointmentService.Domain.Appointments;

namespace CC.AppointmentService.Domain.Feedbacks.Rules;

public static class FeedbackCreationRules
{
    public const int CreationWindowDays = 2;

    public static bool IsWithinCreationWindow(Appointment appointment, DateTime nowUtc) =>
        appointment.EndedAt.HasValue && nowUtc <= appointment.EndedAt.Value.AddDays(CreationWindowDays);
}