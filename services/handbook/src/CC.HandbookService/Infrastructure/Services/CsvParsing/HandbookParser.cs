using System.Globalization;
using CC.HandbookService.Application.Dependencies;
using CC.HandbookService.Domain.Handbooks;
using CC.HandbookService.Infrastructure.Services.CsvParsing.HandbookMaps;
using CsvHelper;
using FluentResults;
using MissingFieldException = CsvHelper.MissingFieldException;

namespace CC.HandbookService.Infrastructure.Services.CsvParsing;

public class HandbookParser : IHandbookParser
{
    public async Task<Result<IEnumerable<T>>> ParseAsync<T>(Stream stream) 
        where T : HandbookItem
    {
        var streamReader = new StreamReader(stream);
        using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);
        csvReader.Context.RegisterClassMap<HandbookItemMap>();

        var records = new List<T>();
        try
        {
            await foreach (var record in csvReader.GetRecordsAsync<T>())
                records.Add(record);
        }
        catch (MissingFieldException ex)
        {
            var cellName = TryGetCellName(ex);
            return cellName != null
                ? Result.Fail($"Пропущенно поле в ячейке {cellName}")
                : Result.Fail("Непредвиденная ошибка");
        }
        catch (ValidationException ex)
        {
            var cellName = TryGetCellName(ex);
            return cellName != null
                ? Result.Fail($"Не заполнено обязательное поле в ячейке {cellName}")
                : Result.Fail("Непредвиденная ошибка");
        }
        
        return Result.Ok<IEnumerable<T>>(records);
    }
    
    private static string? TryGetCellName(CsvHelperException ex)
    {
        if (ex.Context == null || ex.Context.Parser == null || ex.Context.Reader == null)
            return null;
        return CsvCellHelper.GetCellName(ex.Context.Parser.Row, ex.Context.Reader.CurrentIndex + 1);
    }
}