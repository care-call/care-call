using CC.HandbookService.Application.Import;

namespace CC.HandbookService.Infastructure;

public class HandbookInMemoryStore
{
    private readonly Dictionary<HandbookType, List<ImportFile>> _storage = new();

    public void Save(HandbookType handbookType, List<ImportFile> importFiles)
    {
        if (!_storage.ContainsKey(handbookType))
        {
            _storage[handbookType] = new List<ImportFile>();
        }

        _storage[handbookType].AddRange(importFiles);
    }

    public IReadOnlyList<ImportFile> Get(HandbookType handbookType)
    {
        return _storage.TryGetValue(handbookType, out var items) ? items : new List<ImportFile>();
    }
}
