namespace CC.AppointmentService.Domain.Appointments;

/// <summary>
/// Политика записи.
/// <para><b><see cref="MinLeadTime"/></b> — минимум от создания/переноса до начала.</para>
/// <para><b><see cref="MaxTransferShift"/></b> — максимальный сдвиг относительно исходного слота.</para>
/// <para><b><see cref="MinBreakBetweenAppointments"/></b> — минимальный перерыв между записями клиента.</para>
/// <para><b><see cref="MinCancellationNotice"/></b> — минимум до начала, когда ещё можно отменить.</para>
/// <para><see cref="Default"/> используется для стандартных правил.</para>
/// </summary>
public sealed record AppointmentPolicy(
    TimeSpan MinLeadTime,
    TimeSpan MaxTransferShift,
    TimeSpan MinBreakBetweenAppointments,
    TimeSpan MinCancellationNotice)
{
    public static readonly AppointmentPolicy Default = new(
        MinLeadTime: TimeSpan.FromHours(4),
        MaxTransferShift: TimeSpan.FromDays(7),
        MinBreakBetweenAppointments: TimeSpan.FromMinutes(30),
        MinCancellationNotice: TimeSpan.FromHours(1));
}
