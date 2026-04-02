using CC.HandbookService.Domain.Handbooks;
using CsvHelper.Configuration;

namespace CC.HandbookService.Infrastructure.Handbook.HandbookMaps;

public sealed class HandbookItemMap : ClassMap<HandbookItem>
{
    public HandbookItemMap()
    {
        Map(m => m.Code).Validate(args => !args.Field.IsWhiteSpace() && args.Field.Length > 0);
        Map(m => m.DisplayName).Validate(args => !args.Field.IsWhiteSpace() && args.Field.Length > 0);
    }
}