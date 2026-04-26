namespace CC.HandbookService.Domain.Handbooks;

/// <summary>
/// Справочная информация о целевой возрастной группе.
/// </summary>
public class AgeGroup : HandbookItem
{
    public required int FromAge { get; set; }
    public required int ToAge { get; set; }
    public override void UpdateFrom(HandbookItem item)
    {
        base.UpdateFrom(item);
        if (item is AgeGroup ageGroup)
        {
            FromAge = ageGroup.FromAge;
            ToAge = ageGroup.ToAge;
        }
    }
}