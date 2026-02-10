namespace CC.HandbookService.Domain.Handbooks;

/// <summary>
/// Элемент справочника с поддержкой версионности.
/// </summary>
public abstract class VersionedHandbookItem : HandbookItem
{
    public HandbookVersion Version { get; set; }
}