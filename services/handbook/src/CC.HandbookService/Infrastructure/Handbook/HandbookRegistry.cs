using CC.HandbookService.Application.Handbook;
using CC.HandbookService.Domain.Handbooks;
using FluentResults;

namespace CC.HandbookService.Infrastructure.Handbook;

public class HandbookRegistry : IHandbookRegistry
{
    private readonly Dictionary<string, Func<Stream, Task<Result>>> _handbookLoaders = [];

    public HandbookRegistry(IHandbookLoader loader)
    {
        AddHandbookLoader<Language>("languages", loader);
        AddHandbookLoader<AgeGroup>("agegroups", loader);
        AddHandbookLoader<ProblemArea>("problemareas", loader);
    }

    public Func<Stream, Task<Result>>? GetHandbookLoader(string handbookTitle)
    {
        if (_handbookLoaders.TryGetValue(handbookTitle, out var handler))
            return handler;
        
        return null;
    }

    private void AddHandbookLoader<HandbookType>(
        string handbookTitle,
        IHandbookLoader loader) where HandbookType : HandbookItem
    {
        _handbookLoaders[handbookTitle] = loader.LoadAsync<HandbookType>;
    }
}