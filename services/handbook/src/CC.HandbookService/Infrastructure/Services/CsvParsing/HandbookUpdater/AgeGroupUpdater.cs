using CC.HandbookService.Domain.Handbooks;

namespace CC.HandbookService.Infrastructure.Services.CsvParsing.HandbookUpdater;

public sealed class AgeGroupUpdater : HandbookUpdater<AgeGroup>
{
    public override void Update(AgeGroup fromItem, AgeGroup toItem)
    {
        base.Update(fromItem, toItem);
        toItem.FromAge = fromItem.FromAge;
        toItem.ToAge = fromItem.ToAge;
    }
}