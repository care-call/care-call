namespace CC.HandbookService.Domain.Handbooks;

/// <summary>
/// Версия справочника.
/// </summary>
public class HandbookVersion
{
    public HandbookVersion(int id, int version)
    {
        if (version <= 0)
            throw new ArgumentOutOfRangeException(nameof(version));
        Id = id;
        Version = version;
    }
    public int Id { get; protected set; }
    public int Version { get; private set; }
    public bool IsActive { get; set; }
}