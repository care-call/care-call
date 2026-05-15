using FluentResults;

namespace CC.HandbookService.Infrastructure.Services.CsvParsing.Errors;

public static class ParserErrors
{
    public static Error MissingField(string cellName)
        => new ($"Пропущенно поле в ячейке {cellName}");
    
    public static Error EmptyField(string cellName)
        => new ($"Не заполнено обязательное поле в ячейке {cellName}");

    public static Error InvalidFormat(string cellName)
        => new ($"Неверный формат данных в ячейке {cellName}");

    public static Error AgeOutOfRange(string cellName)
        => new ($"Значение возраста должно быть в диапазоне 1..120 в ячейке {cellName}");

    public static Error InvalidAgeRange(string cellName)
        => new ($"FromAge не может быть больше ToAge в ячейке {cellName}");
}