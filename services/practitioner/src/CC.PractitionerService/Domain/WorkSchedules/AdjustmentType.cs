namespace CC.PractitionerService.Domain.WorkSchedules;

public enum AdjustmentType
{
    /// <summary>
    /// Не работает в указанный период.
    /// </summary>
    Unavailable ,

    /// <summary>
    /// Полностью заменяет базовый график на эту дату.
    /// </summary>
    Override 
}