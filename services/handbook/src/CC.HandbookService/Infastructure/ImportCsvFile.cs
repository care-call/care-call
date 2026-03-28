using System.Globalization;
using CC.HandbookService.Application.Import;
using CC.HandbookService.Validate;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
using Microsoft.EntityFrameworkCore;

namespace CC.HandbookService.Infastructure;

public class ImportCsvFile(ValidateCsvFile validate, HandbookDbContext dbContext) : ICsvImportService
{
    private sealed class CsvInputModel
    {
        [Name("id")]
        public string? Id { get; init; }

        [Name("name")]
        public string? Name { get; init; }

        [Name("code")]
        public string? Code { get; init; }
    }

    public async Task<ResultImportFile> ImportAsyncFile(Stream file, HandbookType handbookType, CancellationToken token = default)
    {
        var error = new List<ImportValidateError>();
        var importFile = new List<ImportFile>();

        using var reader = new StreamReader(file);
        var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = args => args.Header.Trim().ToLowerInvariant()
        };
        using var csv = new CsvReader(reader, csvConfiguration);

        try
        {
            await foreach (var row in csv.GetRecordsAsync<CsvInputModel>(token))
            {
                importFile.Add(new ImportFile
                {
                    Id = row.Id?.Trim(),
                    Name = row.Name?.Trim(),
                    Code = row.Code?.Trim()
                });
            }
        }
        catch (HeaderValidationException)
        {
            error.Add(new ImportValidateError
            {
                LineNumber = 1,
                ColumnName = "header",
                Code = "file_invalid_header",
                Message = "CSV header must contain: id,name,code"
            });

            return new ResultImportFile
            {
                ImportFileCount = 0,
                ImportedCount = 0,
                Errors = error
            };
        }
        catch (ReaderException)
        {
            error.Add(new ImportValidateError
            {
                LineNumber = 0,
                ColumnName = string.Empty,
                Code = "file_invalid",
                Message = "The file is not valid"
            });

            return new ResultImportFile
            {
                ImportFileCount = 0,
                ImportedCount = 0,
                Errors = error
            };
        }

        for (var i = 0; i < importFile.Count; i++)
        {
            error.AddRange(validate.ValidateLine(importFile[i], i + 2, handbookType));
        }

        error.AddRange(validate.ValidateLines(importFile, handbookType));

        var invalidLineNumbers = error.Select(x => x.LineNumber).Where(x => x > 0).ToHashSet();
        var validImportFile = importFile
            .Where((_, index) => !invalidLineNumbers.Contains(index + 2))
            .ToList();

        if (validImportFile.Count > 0)
        {
            var externalIds = validImportFile
                .Select(x => x.Id!)
                .Distinct(StringComparer.Ordinal)
                .ToList();

            var existingRecords = await dbContext.HandbookRecords
                .Where(x => x.HandbookType == handbookType && externalIds.Contains(x.ExternalId))
                .ToListAsync(token);

            var byExternalId = existingRecords.ToDictionary(x => x.ExternalId, StringComparer.Ordinal);

            foreach (var item in validImportFile)
            {
                if (byExternalId.TryGetValue(item.Id!, out var existing))
                {
                    existing.Name = item.Name!;
                    existing.Code = item.Code!;
                    continue;
                }

                dbContext.HandbookRecords.Add(new HandbookRecord
                {
                    HandbookType = handbookType,
                    ExternalId = item.Id!,
                    Name = item.Name!,
                    Code = item.Code!
                });
            }

            try
            {
                await dbContext.SaveChangesAsync(token);
            }
            catch (DbUpdateException)
            {
                error.Add(new ImportValidateError
                {
                    LineNumber = 0,
                    ColumnName = string.Empty,
                    Code = "database_conflict",
                    Message = "One or more records could not be imported due to a database constraint conflict."
                });

                return new ResultImportFile
                {
                    ImportFileCount = importFile.Count,
                    ImportedCount = 0,
                    Errors = error
                };
            }
        }

        return new ResultImportFile
        {
            ImportFileCount = importFile.Count,
            ImportedCount = validImportFile.Count,
            Errors = error
        };
    }
}