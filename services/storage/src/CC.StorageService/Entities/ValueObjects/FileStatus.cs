using CC.Shared.Domain;
using CC.Shared.Domain.Exceptions;

namespace CC.StorageService.Entities.ValueObjects;

public sealed record FileStatus
{
    public static readonly FileStatus Temporary = new("temporary", true, true, true);
    public static readonly FileStatus Linked = new("linked", false, true, true);
    public static readonly FileStatus Deleted = new("deleted", false, false, false);

    public string Value { get; }
    public bool CanBeLinked { get; }
    public bool CanBeDeleted { get; }
    public bool CanBeDownloaded { get; }

    private FileStatus(string value, bool canLink, bool canDelete, bool canDownload)
    {
        Value = value;
        CanBeLinked = canLink;
        CanBeDeleted = canDelete;
        CanBeDownloaded = canDownload;
    }

    public static FileStatus FromString(string status) => status switch
    {
        "temporary" => Temporary,
        "linked" => Linked,
        "deleted" => Deleted,
        _ => throw new DomainException($"Некорректный статус файла: {status}")
    };

    public override string ToString() => Value;
}