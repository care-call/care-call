namespace CC.HandbookService.Domain.Handbooks;

/// <summary>
/// Справочная информация о целевой возрастной группе.
/// </summary>
public class AgeGroup : HandbookItem
{
    public int FromAge { get; set; }
    public int ToAge { get; set; }
}