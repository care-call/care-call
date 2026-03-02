namespace CC.AppointmentService.Domain.Appointments.Rules;

public static class AppointmentsTimeRules
{
    public const int MinHoursBeforeStart = 4;
    public const int MaxTransferringShiftDays = 7;
    public const int MinBreakBetweenAppointmentsMinutes = 30;
    public const int MinHoursBeforeCancellation = 1;

    public static bool IsTransferAllowed(Appointment appointment, DateTime now) =>
        (appointment.TimeSlot.From - now).TotalHours < MinHoursBeforeStart;

    public static bool IsCreationAllowed(Appointment appointment, DateTime now) =>
        (appointment.TimeSlot.From - now).TotalHours < MinHoursBeforeStart;

    public static bool IsWithinAllowedShift(Appointment appointment, DateTime proposed) =>
        Math.Abs((proposed - appointment.TimeSlot.From).TotalDays) <= MaxTransferringShiftDays;

    public static bool IsCancellationAllowed(Appointment appointment, DateTime now) =>
        (appointment.TimeSlot.From - now).TotalHours > MinHoursBeforeCancellation;

    public static bool HasInsufficientBreak(Appointment lastAppointment, Appointment appointment) =>
        (appointment.TimeSlot.From - lastAppointment.TimeSlot.From).TotalMinutes < MinBreakBetweenAppointmentsMinutes;
}