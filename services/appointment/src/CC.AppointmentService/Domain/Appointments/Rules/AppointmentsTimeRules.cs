namespace CC.AppointmentService.Domain.Appointments.Rules;

public static class AppointmentsTimeRules
{
    public const int MinHoursBeforeStart = 4;
    public const int MaxTransferringShiftDays = 7;
    public const int MinBreakBetweenAppointmentsMinutes = 30;
    public const int MinHoursBeforeCancellation = 1;
    
    public static bool IsTransferAllowed(DateTime slotStart, DateTime now) =>
        (slotStart - now).TotalHours < MinHoursBeforeStart;
    
    public static bool IsWithinAllowedShift(DateTime original, DateTime proposed) =>
        Math.Abs((proposed - original).TotalDays) <= MaxTransferringShiftDays;
    
    public static bool IsCancellationAllowed(DateTime slotStart, DateTime now) =>
        (slotStart - now).TotalHours > MinHoursBeforeCancellation;
    
    public static bool HasInsufficientBreak(DateTime previousEnd, DateTime newStart) =>
        (newStart - previousEnd).TotalMinutes < MinBreakBetweenAppointmentsMinutes;
}