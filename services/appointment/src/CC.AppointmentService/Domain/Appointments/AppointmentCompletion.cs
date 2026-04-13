namespace CC.AppointmentService.Domain.Appointments;

/// <summary>
/// Факт завершения записи. Присутствует только у завершённых записей.
/// </summary>
public sealed record AppointmentCompletion(DateTime EndedAt);
