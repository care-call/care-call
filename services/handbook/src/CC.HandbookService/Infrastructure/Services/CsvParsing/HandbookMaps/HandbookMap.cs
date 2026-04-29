using CC.HandbookService.Domain.Handbooks;
using CsvHelper.Configuration;

namespace CC.HandbookService.Infrastructure.Services.CsvParsing.HandbookMaps;

public abstract class HandbookMap<T> : ClassMap<T>
    where T : HandbookItem
{
    public HandbookMap()
    {
        #pragma warning disable CA2214 // Не вызывать переопределенный метод в конструкторе
        Map(m => m.Code).Validate(args => !args.Field.IsWhiteSpace() && args.Field.Length > 0);
        Map(m => m.DisplayName).Validate(args => !args.Field.IsWhiteSpace() && args.Field.Length > 0);
        Map(m => m.IsActive).Ignore();
        #pragma warning restore CA2214
    }
}