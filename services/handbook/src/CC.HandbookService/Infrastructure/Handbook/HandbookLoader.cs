using System.Data;
using System.Globalization;
using CC.HandbookService.Application.Csv;
using CsvHelper;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CC.HandbookService.Infrastructure.Csv;

public class CsvLoader(DbContext context) : ICsvLoader
{
    public async Task<Result> LoadFileAsync<T>(Stream stream)
    {
        var streamReader = new StreamReader(stream);
        var csvReader = new CsvReader(streamReader, CultureInfo.CurrentCulture);

        try
        {
            
        }
        catch (CsvHelperException ex)
        {
            
        }
        
        return Result.Ok()
    }

    private static string GetCellName(int index)
    {
                
    }
}