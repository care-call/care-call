namespace CC.HandbookService.Domain.Handbooks;

/// <summary>
/// Справочная информация о языке общения.
/// </summary>
public class Language : HandbookItem
{
    public override void UpdateFrom(HandbookItem item)
    {
        base.UpdateFrom(item);
        if (item is Language language)
        {
        }
    }
}