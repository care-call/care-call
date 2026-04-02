using CC.HandbookService.Domain.Handbooks;
using CsvHelper.Configuration;

namespace CC.HandbookService.Infrastructure.Handbook.HandbookMaps;

public sealed class HandbookMap : ClassMap<HandbookItem>
{
    public HandbookMap()
    {
        Map(m => m.Code).Validate(args => !args.Field.IsWhiteSpace() && args.Field.Length > 0);
        Map(m => m.DisplayName).Validate(args => !args.Field.IsWhiteSpace() && args.Field.Length > 0);
    }
}