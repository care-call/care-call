using CC.HandbookService.Application.Dependencies;
using CC.HandbookService.Domain.Handbooks;
using CC.HandbookService.Domain.Repositories;
using FluentResults;

namespace CC.HandbookService.Infrastructure.Services;

public class HandbookLoader<T> (
    IHandbookParser parser,
    IHandbookRepository<T> repository) : IHandbookLoader 
    where T : HandbookItem 
{
    public async Task<Result> LoadAsync(Stream stream)
    {
        var result = await parser.ParseAsync<T>(stream);
        if (!result.IsSuccess)
            return Result.Fail(result.Errors);
        
        await repository.DeleteAllAsync();
        await repository.AddRangeAsync(result.Value);

        return Result.Ok();
    }
}