using System.Globalization;
using CC.HandbookService.Application.Dependencies;
using CC.HandbookService.Domain.Handbooks;
using CC.HandbookService.Infrastructure.Persistence;
using CC.HandbookService.Infrastructure.Services.CsvParsing;
using CC.HandbookService.Infrastructure.Services.CsvParsing.HandbookMaps;
using CsvHelper;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using MissingFieldException = CsvHelper.MissingFieldException;

namespace CC.HandbookService.Infrastructure.Services;

public class HandbookLoader<T> (
    IHandbookParser parser,
    DatabaseContext context) : IHandbookLoader 
    where T : HandbookItem 
{
    public async Task<Result> LoadAsync(Stream stream)
    {
        var result = await parser.ParseAsync<T>(stream);
        if (!result.IsSuccess)
            return Result.Fail(result.Errors);
        
        await context.Set<T>().ExecuteDeleteAsync();
        context.Set<T>().AddRange(result.Value); 

        return Result.Ok();
    }
}