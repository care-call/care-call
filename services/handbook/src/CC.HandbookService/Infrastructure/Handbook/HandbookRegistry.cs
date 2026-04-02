using CC.HandbookService.Application.Handbook;
using CC.HandbookService.Domain.Handbooks;

namespace CC.HandbookService.Infrastructure.Csv;

public class HandbookRegistry : IHandbookRegistry
{
    private readonly List<string> _handbooks;

    public HandbookRegistry()
    {
        var assembly = typeof(HandbookItem).Assembly;
        
        var handbookTypes = assembly
            .GetTypes()
            .Where(type => type.IsSubclassOf(typeof(HandbookItem)));
        
        _handbooks = handbookTypes.Select(type => type.Name).ToList();
    }
}