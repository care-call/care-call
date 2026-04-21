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
    private static readonly Dictionary<Type, Delegate> _updaters = new()
    {
        [typeof(Language)] = (Action<Language, Language>)(static (d, n) =>
        {
            UpdateBaseFields(d, n);
        }),
        [typeof(AgeGroup)] = (Action<AgeGroup, AgeGroup>)(static (d, n) =>
        {
            UpdateBaseFields(d, n);
            d.FromAge = n.FromAge;
            d.ToAge = n.ToAge;
        }),
        [typeof(ProblemArea)] = (Action<ProblemArea, ProblemArea>)(static (d, n) =>
        {
            UpdateBaseFields(d, n);
        })
    };

    public async Task<Result> LoadAsync(Stream stream)
    {
        var result = await parser.ParseAsync<T>(stream);
        if (!result.IsSuccess)
            return Result.Fail(result.Errors);

        var items = result.Value;

        await repository.SetAllInActiveStatus();

        var entities = await repository.GetByCodes([.. items.Select(x => x.Code)]);

        if (!_updaters.TryGetValue(typeof(T), out var updater))
            throw new Exception($"Не найден update для типа {typeof(T)}");

        foreach (var item in items)
        {
            item.IsActive = true;

            if (entities.TryGetValue(item.Code, out var entity))
                ((Action<T, T>)updater)((T)entity, (T)item);
            else
                await repository.AddRangeAsync(items);
        }

        return Result.Ok();
    }

    private static void UpdateBaseFields<TItem>(TItem d, TItem n) 
        where TItem : HandbookItem => d.DisplayName = n.DisplayName;
}