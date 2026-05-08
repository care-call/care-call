using System.Globalization;
using CC.HandbookService.Application.Dependencies;
using CC.HandbookService.Domain.Errors;
using CC.HandbookService.Domain.Handbooks;
using CC.HandbookService.Infrastructure.Services.CsvParsing.Errors;
using CC.HandbookService.Infrastructure.Services.CsvParsing.HandbookMaps;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using FluentResults;
using MissingFieldException = CsvHelper.MissingFieldException;

namespace CC.HandbookService.Infrastructure.Services.CsvParsing;

public class HandbookParser(IServiceProvider keyedProvider) : IHandbookParser
{
    public async Task<Result<IEnumerable<T>>> ParseAsync<T>(Stream stream) 
        where T : HandbookItem
    {
        var streamReader = new StreamReader(stream);
        using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);
        var map = keyedProvider.GetRequiredKeyedService<ClassMap>(typeof(T));
        csvReader.Context.RegisterClassMap(map);

        var records = new List<T>();
        try
        {
            await foreach (var record in csvReader.GetRecordsAsync<T>())
                records.Add(record);
            
        }
        catch (MissingFieldException ex)
        {
            var cellName = GetCellName(ex);
            return Result.Fail(ParserErrors.MissingField(cellName));
        }
        catch (ValidationException ex)
        {
            var cellName = GetCellName(ex);
            return Result.Fail(ParserErrors.EmptyField(cellName));
        }
        catch(TypeConverterException ex)
        {
            var cellName = GetCellName(ex);
            return Result.Fail(ParserErrors.InvalidFormat(cellName));
        }
        
        return Result.Ok<IEnumerable<T>>(records);
    }
    
    private static string GetCellName(CsvHelperException ex)
    {
        if (ex.Context == null || ex.Context.Parser == null || ex.Context.Reader == null)
            throw new InvalidOperationException("Parser context is null");
        return CsvCellHelper.GetCellName(ex.Context.Parser.Row, ex.Context.Reader.CurrentIndex + 1);
    }
}