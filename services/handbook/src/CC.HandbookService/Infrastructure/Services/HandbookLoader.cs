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

        var items = result.Value
            .GroupBy(x => x.Code)
            .Select(y => y.Last());

        var itemsCodes = items.Select(x => x.Code);
        
        var entities = await repository.GetByCodes(itemsCodes);

        var newItems = new List<T>();

        foreach (var item in items)
        {
            item.IsActive = true;

            if (entities.TryGetValue(item.Code, out var entity))
                entity.UpdateFrom(item);
            else
                newItems.Add(item);
        }

        if (newItems.Count != 0)
            await repository.AddRangeAsync(newItems);

        await repository.SetInactiveStatusByMissingCodesAsync(itemsCodes);

        return Result.Ok();
    }
}