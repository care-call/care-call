using CC.HandbookService.Domain.Handbooks;
using CsvHelper.Configuration;

namespace CC.HandbookService.Infrastructure.Services.CsvParsing.HandbookMaps
{
    public sealed class AgeGroupMap : HandbookItemMap<AgeGroup>
    {
        public AgeGroupMap()
        {
            Map(m => m.FromAge).Validate(args => !string.IsNullOrWhiteSpace(args.Field));
            Map(m => m.ToAge).Validate(args => !string.IsNullOrWhiteSpace(args.Field));
        }
    }
}
