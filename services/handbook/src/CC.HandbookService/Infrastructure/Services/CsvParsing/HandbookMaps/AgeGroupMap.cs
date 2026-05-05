using CC.HandbookService.Domain.Handbooks;
using CsvHelper.Configuration;

namespace CC.HandbookService.Infrastructure.Services.CsvParsing.HandbookMaps;

public sealed class AgeGroupMap : HandbookMap<AgeGroup>
{
    public AgeGroupMap()
    {
        Map(m => m.FromAge)
            .Validate(args => int.TryParse(args.Field, out var value) && value >= 1 && value <= 120);

        Map(m => m.ToAge)
            .Validate(args => int.TryParse(args.Field, out var value) && value >= 1 && value <= 120);
    }
}