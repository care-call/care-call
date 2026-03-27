namespace CC.HandbookService.Validate;

using CC.HandbookService.Application.Import;

public class ValidateCsvFile
{
    /// <summary>
    /// Валидация строк
    /// </summary>
    public List<ImportValidateError> ValidateLine(ImportFile importFile, int lineNumber)
    {
        var errors = new List<ImportValidateError>();

        if (string.IsNullOrWhiteSpace(importFile.Id))
        {
            errors.Add(new ImportValidateError
            {
                LineNumber = lineNumber,
                ColumnName = nameof(importFile.Id),
                Code = "file_empty",
                Message = "Id is required"
            });
        }

        if (string.IsNullOrWhiteSpace(importFile.Name))
        {
            errors.Add(new ImportValidateError
            {
                LineNumber = lineNumber,
                ColumnName = nameof(importFile.Name),
                Code = "file_empty",
                Message = "Name is required"
            });
        }

        if (string.IsNullOrWhiteSpace(importFile.Code))
        {
            errors.Add(new ImportValidateError
            {
                LineNumber = lineNumber,
                ColumnName = nameof(importFile.Code),
                Code = "file_empty",
                Message = "Code is required"
            });
        }

        return errors;
    }

    public List<ImportValidateError> ValidateLine(ImportFile importFile, int lineNumber, HandbookType handbookType)
    {
        return ValidateLine(importFile, lineNumber);
    }

    /// <summary>
    /// Проверка дубликатов
    /// </summary>
    public List<ImportValidateError> ValidateLines(List<ImportFile> importFiles)
    {
        var errors = new List<ImportValidateError>();
        var seenKeys = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        for (int i = 0; i < importFiles.Count; i++)
        {
            var importFile = importFiles[i];
            var key = $"{importFile.Id} {importFile.Code}";

            if (seenKeys.TryGetValue(key, out var firstOccurrenceLineNumber))
            {
                errors.Add(new ImportValidateError
                {
                    LineNumber = i + 2,
                    ColumnName = nameof(importFile.Code),
                    Code = "duplicate_key",
                    Message = $"Duplicate key found. First occurrence at line {firstOccurrenceLineNumber}"
                });
            }
            else
            {
                seenKeys[key] = i + 2;
            }
        }
        return errors;
    }

    public List<ImportValidateError> ValidateLines(List<ImportFile> importFiles, HandbookType handbookType)
    {
        var errors = new List<ImportValidateError>();
        var seenCodes = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        var seenIds = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        for (int i = 0; i < importFiles.Count; i++)
        {
            var importFile = importFiles[i];
            var code = importFile.Code?.Trim() ?? string.Empty;
            var id = importFile.Id?.Trim() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(code) && seenCodes.TryGetValue(code, out var firstCodeLineNumber))
            {
                errors.Add(new ImportValidateError
                {
                    LineNumber = i + 2,
                    ColumnName = nameof(importFile.Code),
                    Code = "duplicate_code",
                    Message = $"Duplicate code found. First occurrence at line {firstCodeLineNumber}"
                });
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(code))
                {
                    seenCodes[code] = i + 2;
                }
            }

            if (!string.IsNullOrWhiteSpace(id) && seenIds.TryGetValue(id, out var firstIdLineNumber))
            {
                errors.Add(new ImportValidateError
                {
                    LineNumber = i + 2,
                    ColumnName = nameof(importFile.Id),
                    Code = "duplicate_id",
                    Message = $"Duplicate id found. First occurrence at line {firstIdLineNumber}"
                });
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(id))
                {
                    seenIds[id] = i + 2;
                }
            }
        }

        return errors;
    }
}