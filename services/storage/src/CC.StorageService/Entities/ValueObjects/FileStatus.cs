using CC.Shared.Domain;
using CC.Shared.Domain.Exceptions;

namespace CC.StorageService.Entities.ValueObjects;

public sealed class FileStatus : ValueObject
{
    public static readonly FileStatus Temporary = new("temporary", canLink: true, canDelete: true, canDownload: true);
    public static readonly FileStatus Linked = new("linked", canLink: false, canDelete: true, canDownload: true);
    public static readonly FileStatus Deleted = new("deleted", canLink: false, canDelete: false, canDownload: false);

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

    public static FileStatus FromString(string status)
    {
        return status switch
        {
            "temporary" => Temporary,
            "linked" => Linked,
            "deleted" => Deleted,
            _ => throw new DomainException($"Invalid file status: {status}")
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}