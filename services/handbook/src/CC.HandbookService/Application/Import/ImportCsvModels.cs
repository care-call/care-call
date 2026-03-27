namespace CC.HandbookService.Application.Import;

public enum HandbookType
{
    Language,
    Age,
    Problems
}

public sealed class ImportFile
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Code { get; init; }
}

public sealed class ImportValidateError
{
    public int LineNumber { get; init; }
    public string ColumnName { get; init; }
    public string Code { get; init; }
    public string Message { get; init; } = string.Empty;
}

public sealed class ResultImportFile
{
    public int ImportFileCount { get; init; }
    public int ImportedCount { get; init; }
    public List<ImportValidateError> Errors { get; init; } = new();
    public bool Success => Errors.Count == 0;
}