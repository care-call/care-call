namespace CC.AppointmentService.Domain.Appointments.Rules;

public static class AppointmentsTimeRules
{
    public static readonly TimeSpan MinHoursBeforeAppointment = TimeSpan.FromHours(4);
    public static readonly TimeSpan MaxDaysTransferAppointment = TimeSpan.FromDays(7);
    public static readonly TimeSpan MinMinutesBetweenAppointments = TimeSpan.FromMinutes(30);
}