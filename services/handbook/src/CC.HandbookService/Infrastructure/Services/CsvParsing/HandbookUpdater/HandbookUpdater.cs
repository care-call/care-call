using CC.HandbookService.Application.Dependencies;
using CC.HandbookService.Domain.Handbooks;

namespace CC.HandbookService.Infrastructure.Services.CsvParsing.HandbookUpdater;

public abstract class HandbookUpdater<T> : IHandbookUpdater<T>
    where T : HandbookItem
{
    public virtual void Update(T fromItem, T toItem) => toItem.DisplayName = fromItem.DisplayName;
}