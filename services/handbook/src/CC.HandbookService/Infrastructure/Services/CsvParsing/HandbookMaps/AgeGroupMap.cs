using CC.HandbookService.Domain.Handbooks;

namespace CC.HandbookService.Infrastructure.Services.CsvParsing.HandbookMaps;

public sealed class AgeGroupMap : HandbookMap<AgeGroup>
{
    public AgeGroupMap()
    {
        Map(m => m.FromAge).Validate(args => int.TryParse(args.Field, out var _));
        Map(m => m.ToAge).Validate(args => int.TryParse(args.Field, out var _));
    }
}