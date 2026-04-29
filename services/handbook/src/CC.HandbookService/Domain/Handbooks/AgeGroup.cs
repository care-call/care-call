namespace CC.HandbookService.Domain.Handbooks;

/// <summary>
/// Справочная информация о целевой возрастной группе.
/// </summary>
public class AgeGroup : HandbookItem
{
    public required int FromAge { get; set; }
    public required int ToAge { get; set; }
}