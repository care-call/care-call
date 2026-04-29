using CC.HandbookService.Domain.Handbooks;

namespace CC.HandbookService.Application.Dependencies;

public interface IHandbookUpdater<T>
    where T : HandbookItem
{
    public void Update(T fromItem, T toItem);
}