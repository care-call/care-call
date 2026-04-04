using CC.HandbookService.Application.Dependencies.Handbook;
using CC.HandbookService.Domain.Handbooks;
using FluentResults;

namespace CC.HandbookService.Infrastructure.Services.Handbook;

public class HandbookRegistry : IHandbookRegistry
{
    private readonly Dictionary<HandbookType, Func<Stream, Task<Result>>> _handbookLoaders = [];

    public HandbookRegistry(IHandbookLoader loader)
    {
        AddHandbookLoader<Language>(HandbookType.Languages, loader);
        AddHandbookLoader<AgeGroup>(HandbookType.AgeGroups, loader);
        AddHandbookLoader<ProblemArea>(HandbookType.ProblemAreas, loader);
    }

    public Func<Stream, Task<Result>> GetHandbookLoader(HandbookType handbookType)
        => _handbookLoaders[handbookType];

    private void AddHandbookLoader<H>(
        HandbookType handbookType,
        IHandbookLoader loader) where H : HandbookItem
    {
        _handbookLoaders[handbookType] = loader.LoadAsync<H>;
    }
}