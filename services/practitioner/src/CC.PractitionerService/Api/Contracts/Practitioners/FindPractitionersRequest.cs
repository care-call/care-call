namespace CC.PractitionerService.Api.Contracts.Practitioners;

/// <summary>
/// Запрос на подбор практиканта для записи.
/// </summary>
public sealed record FindPractitionersRequest
{
    /// <summary>
    /// Интересующая дата для клиента, на которую желает записаться.
    /// </summary>
    public DateOnly? TargetDate { get; init; }

    /// <summary>
    /// Идентификаторы возвростных групп.
    /// </summary>
    public IReadOnlyCollection<int>? AgeGroupIds { get; init; }

    /// <summary>
    /// Идентификаторы проблемных областей.
    /// </summary>
    public IReadOnlyCollection<int>? ProblemAreas { get; init; }

    /// <summary>
    /// Языки, на которых может общаться практикант.
    /// </summary>
    public IReadOnlyCollection<int>? PractitionerLanguages { get; init; }

    /// <summary>
    /// Поиск по ФИО практиканта.
    /// </summary>
    public string? PractitionerFullName { get; init; }
}