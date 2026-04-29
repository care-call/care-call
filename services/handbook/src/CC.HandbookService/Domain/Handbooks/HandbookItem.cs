namespace CC.HandbookService.Domain.Handbooks;

/// <summary>
/// Элемент справочника.
/// </summary>
public abstract class HandbookItem
{
    public int Id { get; init; }
    public required string Code { get; init; }
    public required string DisplayName { get; set; }
    public bool IsActive { get; set; }
}