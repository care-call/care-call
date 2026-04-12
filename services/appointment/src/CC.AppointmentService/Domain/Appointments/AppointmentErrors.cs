using CC.Common.Errors;
using FluentResults;

namespace CC.AppointmentService.Domain.Appointments;

public static class AppointmentErrors
{
    private static readonly AppointmentPolicy Policy = AppointmentPolicy.Default;

    // A1xx - Ошибки валидации времени
    public static Error TooSoon =>
        new Error($"До начала записи должно оставаться не менее {Policy.MinLeadTime.TotalHours} часа(ов)")
            .WithErrorCode("A101");

    public static Error NotWithinAllowedShift =>
        new Error($"Максимальная дальность переноса — {Policy.MaxTransferShift.TotalDays} дн.")
            .WithErrorCode("A102");

    public static Error MinBreakBetweenAppointments =>
        new Error($"Минимальный интервал между записями — {Policy.MinBreakBetweenAppointments.TotalMinutes} минут")
            .WithErrorCode("A103");

    public static Error CancellationDeadlineExceeded =>
        new Error($"Для отмены записи должно оставаться не менее {Policy.MinCancellationNotice.TotalHours} часа(ов)")
            .WithErrorCode("A104");

    // A2xx - Ошибки пересечений и состояния
    public static Error HasIntercepts() =>
        new Error("Клиент не может иметь пересекающиеся записи").WithErrorCode("A201");

    public static Error NotFound() =>
        new Error("Заявка не существует").WithErrorCode("A202");

    public static Error InvalidStatusForTransfer() =>
        new Error("Заявка уже завершена, перенос не возможен").WithErrorCode("A203");

    public static Error InvalidStatusForCancel() =>
        new Error("Заявка уже отменена, отменить снова нельзя").WithErrorCode("A204");

    public static Error InvalidStatusForComplete() =>
        new Error("Заявка готова, завершить ее нельзя").WithErrorCode("A205");
}
