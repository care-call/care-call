using System.Globalization;
using System.Text;
using CC.HandbookService.Application.Dependencies.Handbook;
using CC.HandbookService.Domain.Handbooks;
using CC.HandbookService.Infrastructure.Persistence;
using CC.HandbookService.Infrastructure.Persistence.Handbook.HandbookMaps;
using CsvHelper;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using MissingFieldException = CsvHelper.MissingFieldException;

namespace CC.HandbookService.Infrastructure.Services.Handbook;

public class HandbookLoader(DatabaseContext context) : IHandbookLoader
{
    public async Task<Result> LoadAsync<T>(Stream stream) where T : HandbookItem
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
            
        await context.Set<T>().ExecuteDeleteAsync();
        context.Set<T>().AddRange(records); 

        return Result.Ok();
    }
    
    private static string? TryGetCellName(CsvHelperException ex)
    {
        if (ex.Context == null || ex.Context.Parser == null || ex.Context.Reader == null)
            return null;
        return CsvCellHelper.GetCellName(ex.Context.Parser.Row, ex.Context.Reader.CurrentIndex + 1);
    }
}