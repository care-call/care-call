namespace CC.AppointmentService.Domain.Appointments.Rules;

public static class AppointmentsTimeRules
{
    public static readonly int MinHoursBeforeAppointment = 4;
    public static readonly int MaxDaysTransferAppointment = 7;
    public static readonly int MinMinutesBetweenAppointments = 30;
}