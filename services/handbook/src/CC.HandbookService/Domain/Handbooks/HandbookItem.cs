namespace CC.HandbookService.Domain.Handbooks;

/// <summary>
/// Элемент справочника.
/// </summary>
public abstract class HandbookItem
{
    public string Code { get; set; }
    public string DisplayName { get; set; }
}