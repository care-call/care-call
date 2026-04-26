namespace CC.HandbookService.Domain.Handbooks;

/// <summary>
/// Справочная информация о психологичной проблеме.
/// </summary>
public class ProblemArea : HandbookItem
{
    public override void UpdateFrom(HandbookItem item)
    {
        base.UpdateFrom(item);
        if (item is ProblemArea problemArea)
        {
        }
    }
}