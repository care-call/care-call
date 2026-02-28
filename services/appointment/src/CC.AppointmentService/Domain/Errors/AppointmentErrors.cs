using CC.Common.Errors;
using FluentResults;

namespace CC.AppointmentService.Domain.Errors;

public static class AppointmentErrors
{
    // A1xx - Ошибки валидации времени
    public static Error TooSoon(int hours) =>
        new Error($"Время между созданием записи и началом должно быть не меньше {hours} часа(ов)").WithErrorCode("A101");

    public static Error NotWithinAllowedShift(int days) =>
        new Error($"Максимальная дальность переноса записи не больше {days} дн. от текущей даты").WithErrorCode("A102");

    public static Error MinBreakBetweenAppointments(int minutes) =>
        new Error($"Минимальный интервал между записями — {minutes} минут").WithErrorCode("A103");

    // A2xx - Ошибки пересечений и состояния
    public static Error HasIntercepts() =>
        new Error("Клиент не может иметь пересекающиеся записи").WithErrorCode("A201");

    public static Error NotFound() =>
        new Error("Заявка не существует").WithErrorCode("A202");

    public static Error InvalidStatusForTransfer() =>
        new Error("Заявка уже завершена, перенос не возможен").WithErrorCode("A203");
}